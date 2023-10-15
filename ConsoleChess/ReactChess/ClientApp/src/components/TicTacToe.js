import React, { Component } from 'react';
import { HubConnectionBuilder } from '@microsoft/signalr';
import authService from './api-authorization/AuthorizeService'
import { HttpTransportType } from '@microsoft/signalr';
import { LogLevel } from '@microsoft/signalr';
import { Clock } from 'chess-clock'
import './styles.css';

    const fischer = Clock.getConfig('Fischer Rapid 5|5')
    const updateInterval = 500
    const callback = console.info

var gameConnection = new HubConnectionBuilder()
    .withUrl('https://localhost:7073/chesshub', {
        accessTokenFactory: () => {
            return authService.getAccessToken();
        }, transport: HttpTransportType.WebSockets, skipNegotiation: true
    })
    .configureLogging(LogLevel.Information)
    .build();

    


export class TicTacToe extends Component {
    static displayName = TicTacToe.name;


    constructor(props) {
        super(props);
        this.state = {
            board: [], players: [], loading: true, started: false, joined: false, isWhite: true, turnOf: true, duringMove: false, prevx: 0, prevy: 0, promoteTo: 1, winner: 3,
            possibleMoves: [], prevMoves: [], promotionVisible: false, ownuser: "", otheruser: "", clock: new Clock({
                ...fischer,
                updateInterval,
                callback,
            }),
            whitepoints: 0, blackpoints: 0
        };

        gameConnection.on('AddToGame', (playerlist) => {
            this.setState({ players: playerlist })

            this.forceUpdate()
        })

        gameConnection.on('SetColor', (isWhite, username) => {
            this.setState({ isWhite: isWhite, ownuser: username, joined: true })
            this.forceUpdate()
        })

        gameConnection.on('GameCreated', (board) => {
            this.setState({
                board: board, loading: false, started: true
            })
            for (let i = 0; i < this.state.players.length; i++) {
                if (this.state.players[i] !== this.state.ownuser) {
                    this.setState({ otheruser: this.state.players[i] })
                    break;
                }

            }
            console.log("Game started")
            this.forceUpdate()

        })
        gameConnection.on('RefreshBoard', (board, success) => {
            this.setState({ board: board })

            if (!success) console.log("Rossz lépés")
            else {
                if (this.state.turnOf) this.state.clock.push(0);
                else this.state.clock.push(1);
                this.setState({ turnOf: !this.state.turnOf })
            }
            this.forceUpdate()
        })
        gameConnection.on('GetPossibleMoves', (moves) => {
            this.setState({ possibleMoves: moves })
            this.forceUpdate()
        })
        gameConnection.on('PreviousMoves', (moves) => {
            this.setState({ prevMoves: moves })
            this.forceUpdate()
        })
        gameConnection.on('RefreshPoints', (white, black) => {
            this.setState({ whitepoints: white, blackpoints: black })
            this.forceUpdate()
        })

        gameConnection.on('GameEnds', (result) => {
            this.setState({ winner: result })
            this.forceUpdate()
        })

        gameConnection.onclose(async () => {
            await this.startconnection();
        })

        this.startconnection();
    }

    componentDidMount() {
        this.interval = setInterval(() => this.setState({ time: Date.now() }), 1000);


        this.setState({
            clock: new Clock({
                ...fischer,
                updateInterval,
                callback: async (state) => {
                    if (state.status === "done") {
                        if (this.state.turnOf && this.state.isWhite) {
                            console.log("black wins")
                            await gameConnection.invoke('LoseGame');
                        }
                        else if (!this.state.turnOf && !this.state.isWhite) {
                            console.log("white wins")
                            await gameConnection.invoke('LoseGame');
                        }
                    }
                }
            })
        })
    }
    componentWillUnmount() {
        clearInterval(this.interval);
    }

    renderJoin(players) {
        if (!this.state.joined)
            return (
                <div className='end-screen'>
                    <button className='btn btn-secondary btn-lg' onClick={this.onJoinGame}>Join game</button>
                </div>)
        else return (
            <div className='end-screen'>
                <p>Searching for opponent...</p>
            </div>
        )

    }
    renderMoves() {
        const moves = [];
        for (let i = 0; i < this.state.prevMoves.length; i++) {
            let movestring = this.moveToString(this.state.prevMoves[i]);
            moves.push(movestring)
        }

        const rows = [];
        for (let i = 0; i < Math.ceil(this.state.prevMoves.length / 2); i++) {
            const cols = [];
            for (let j = 0; j < 2; j++) {
                if (i * 2 + j >= moves.length) {
                    cols.push(<td ></td>)
                }
                else cols.push(<td >{moves[i * 2 + j]}</td>)
            }

            rows.push(<tr>{cols}</tr>)

        }

        return (
            <div className='moves'>
                <strong>Moves</strong>
                <table className='moves-table'>
                    <thead>
                        <th>Circle</th>
                        <th>Cross</th>
                    </thead>
                    <tbody>
                        {rows}
                    </tbody>
                </table>

            </div>)
    }

    moveToString(move) {
        
        return move.x+"; "+move.y;
        
    }
    renderBlack() {
        let whiteminutes = Math.floor(this.state.clock.state.remainingTime[0] / 60000).toLocaleString('en-US', { minimumIntegerDigits: 2, useGrouping: false });
        let blackminutes = Math.floor(this.state.clock.state.remainingTime[1] / 60000).toLocaleString('en-US', { minimumIntegerDigits: 2, useGrouping: false });
        let whiteseconds = Math.floor((this.state.clock.state.remainingTime[0] % 60000) / 1000).toLocaleString('en-US', { minimumIntegerDigits: 2, useGrouping: false });
        let blackseconds = Math.floor((this.state.clock.state.remainingTime[1] % 60000) / 1000).toLocaleString('en-US', { minimumIntegerDigits: 2, useGrouping: false });
        const rows = [];
        for (let i = 0; i < 3; i++) {

            const cols = [];
            for (let j = 0; j < 3; j++) {
                
                cols.push(<td><button className='chess' style={this.returnColor((i + j) % 2 === 0, false)} onClick={() => this.onClick(i, j)}>{this.returnText(i, j)}&#xFE0E;</button></td>);
                
            }
            rows.push(<tr>{cols}</tr>)
        }
        return (
            <div>
                <div className='flexing info-bar'>
                    <div className='clock-container'>
                        <strong>{whiteminutes}:{whiteseconds}</strong>
                    </div>
                    <strong>{this.state.otheruser}</strong>
                </div>
                <table style={this.setBackground()} className='table' aria-labelledby="tabelLabel">
                    <tbody>
                        {rows}
                    </tbody>
                </table>
                <div className='flexing info-bar'>
                    <div className='clock-container'>
                        <strong>{blackminutes}:{blackseconds}</strong>
                    </div>
                    <strong>{this.state.ownuser}</strong>
                    <div className='flexing resign-button'><button className='btn btn-secondary' onClick={() => this.onResign()}>Resign</button></div>
                    <div> <strong>{this.state.turnOf === this.state.isWhite ?"Your turn!":"Opponent's turn!"}</strong></div>
                </div>
            </div>)


    }
    returnColor(white, highlighted) {
        if (!highlighted) {
            if (white === true) {
                return { backgroundColor: 'white', color: 'black', width: '100px', height: '100px', border: 'none', borderRadius: '0px 0px 0px 0px', borderWidth: '0px', textAlign: 'center', bottom: '0', fontSize: '80px' };
            }
            return { backgroundColor: 'darkslategray', color: 'black', width: '100px', height: '100px', border: 'none', borderRadius: '0px 0px 0px 0px', borderWidth: '0px', textAlign: 'center', bottom: '0', fontSize: '80px' };
        }
        else {
            if (white === true) {
                return { backgroundColor: 'rgb(255, 213, 128)', color: 'black', width: '100px', height: '100px', border: 'none', borderRadius: '0px 0px 0px 0px', borderWidth: '0px', textAlign: 'center', bottom: '0', fontSize: '80px' };
            }
            return { backgroundColor: 'darkorange', color: 'black', width: '100px', height: '100px', border: 'none', borderRadius: '0px 0px 0px 0px', borderWidth: '0px', textAlign: 'center', bottom: '0', fontSize: '80px' };
        }
    }

    setBackground() {
        return ({
            height: 'fit-content',
            width: 'fit-content',
            backgroundSize: 'cover',
            backgroundPosition: 'center',
            padding: '100px',
            borderCollapse: 'collapse'
        })
    }

    returnText(x, y) {
        //console.log(this.state.board[3 * x + y])
        if (this.state.board[3 * x + y].type === 2) return ""
        else if(this.state.board[3 * x + y].type === 0) return "O"
        else return "X"
    }

    render() {

        let join = this.state.started ? <div></div> : this.renderJoin(this.state.players)
        let content = <div></div>
       
        
        content = this.state.loading ? <div></div> : this.renderBlack()
    
        let win = <div></div>

        switch (this.state.winner) {
            case 0:
                win = <div className='end-screen'>
                    <p>Circle wins!</p>
                    <button className='btn btn-secondary' onClick={this.restart}>OK</button>
                </div>
                break;
            case 1:
                win = <div className='end-screen'>
                    <p>Cross wins!</p>
                    <button className='btn btn-secondary' onClick={this.restart}>OK</button>
                </div>
                break;
            case 2:
                win=<div className='end-screen'>
                    <p>Draw!</p>
                    <button className='btn btn-secondary' onClick={this.restart}>OK</button>
                </div>
                break;
            default:
                break;

        }
        let moves = this.state.loading ? <div></div> : this.renderMoves();

        let all = !this.state.started ? <div>{join}</div> :
            <div className='flexing'>
                {content}
                <div>
                    {moves}
                </div>
            </div>
        if (this.state.winner <= 2) all = win


        return (
            <div className='chess-page'>
                {all}
            </div>

        );
    }



    async startconnection() {

        try {
            await gameConnection.start();
            console.log("SignalR (game) Connected.");
            //await gameConnection.invoke('GameStarted');

        } catch (err) {
            console.log("Start hiba:" + err);
            setTimeout(this.start, 50000);
        }
    }

    onClick = async (x, y) => {
        console.log(x + " " + y + " meg lett nyomva")
        if (this.state.board[3 * x + y].type !== 2 ) return
        if (this.state.turnOf !== this.state.isWhite) return       
        await gameConnection.invoke('MakeMove', JSON.stringify({X: x, Y: y}), 1);
        
    }
    onResign = async () => {
        await gameConnection.invoke('LoseGame', 1);
    }
    onJoinGame = async (e) => {
        e.preventDefault()
        if (!this.state.joined) {
            this.setState({ joined: false })
            await gameConnection.invoke('EnterGame', 1)
        }

    }
    
    restart = () => {
        this.setState({
            board: [], players: [], loading: true, started: false, joined: false, isWhite: true, turnOf: true, duringMove: false, prevx: 0, prevy: 0, promoteTo: 1, winner: 3,
            possibleMoves: [], prevMoves: [], promotionVisible: false, ownuser: "", otheruser: "", clock: new Clock({
                ...fischer,
                updateInterval,
                callback,
            }),
            whitepoints: 0, blackpoints: 0
        })
        this.setState({
            clock: new Clock({
                ...fischer,
                updateInterval,
                callback: async (state) => {
                    if (state.status === "done") {
                        if (this.state.turnOf && this.state.isWhite) {
                            console.log("black wins")
                            await gameConnection.invoke('LoseGame');
                        }
                        else if (!this.state.turnOf && !this.state.isWhite) {
                            console.log("white wins")
                            await gameConnection.invoke('LoseGame');
                        }
                    }
                }
            })
        })
        this.forceUpdate();
    }
  
}