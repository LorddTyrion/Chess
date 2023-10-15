import { Component } from 'react';
import { HubConnectionBuilder } from '@microsoft/signalr';
import authService from './api-authorization/AuthorizeService'
import { HttpTransportType } from '@microsoft/signalr';
import { LogLevel } from '@microsoft/signalr';
import { Clock } from 'chess-clock'



import './styles.css';
import { ProgressContext } from '../context/progress';

const fischer = Clock.getConfig('Fischer Rapid 5|5')
const updateInterval = 500
const callback = console.info


export class BoardComponent extends Component {
    static displayName = BoardComponent.name;
    static contextType=ProgressContext
    constructor(props) {
        super(props);
        this.state = {
            board: [], players: [], loading: true, started: false, joined: false, isWhite: true, turnOf: true, duringMove: false, prevx: 0, prevy: 0, promoteTo: 1, winner: 3,
            possibleMoves: [], prevMoves: [], promotionVisible: false, ownuser: "", otheruser: "", clock: new Clock({
                ...fischer,
                updateInterval,
                callback,
            }),
            gameConnection: new HubConnectionBuilder()
            .withUrl('https://localhost:7073/chesshub', {
                accessTokenFactory: () => {
                    return authService.getAccessToken();
                }, transport: HttpTransportType.WebSockets, skipNegotiation: true
            })
            .configureLogging(LogLevel.Information)
            .build(),
            whitepoints: 0, blackpoints: 0, gameType: 0
        };

        //this.restart=this.restart.bind(this)
        //this.renderJoin=this.renderJoin.bind(this)

        this.state.gameConnection.on('AddToGame', (playerlist) => {
            this.setState({ players: playerlist })

            this.forceUpdate()
        })

        this.state.gameConnection.on('SetColor', (isWhite, username) => {
            this.setState({ isWhite: isWhite, ownuser: username, joined: true })
            this.forceUpdate()
        })

        this.state.gameConnection.on('GameCreated', (board) => {
            
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
            this.context.setProgress(true)
            this.forceUpdate()

        })
        this.state.gameConnection.on('RefreshBoard', (board, success) => {
            this.setState({ board: board })

            if (!success) console.log("Rossz lépés")
            else {
                if (this.state.turnOf) this.state.clock.push(0);
                else this.state.clock.push(1);
                this.setState({ turnOf: !this.state.turnOf })
            }
            this.forceUpdate()
        })
        this.state.gameConnection.on('GetPossibleMoves', (moves) => {
            this.setState({ possibleMoves: moves })
            this.forceUpdate()
        })
        this.state.gameConnection.on('PreviousMoves', (moves) => {
            this.setState({ prevMoves: moves })
            this.forceUpdate()
        })
        this.state.gameConnection.on('RefreshPoints', (white, black) => {
            this.setState({ whitepoints: white, blackpoints: black })
            this.forceUpdate()
        })

        this.state.gameConnection.on('GameEnds', (result) => {
            this.context.setProgress(false)
            this.setState({ winner: result })
            this.forceUpdate()
        })

        this.state.gameConnection.onclose(async () => {
            await this.startconnection();
        })

        this.startconnection();

        
    }
    async startconnection() {

        try {
            await this.state.gameConnection.start();
            console.log("SignalR (game) Connected.");

        } catch (err) {
            console.log("Start hiba:" + err);
            setTimeout(this.start, 50000);
        }
    }
    render(){
        return false
    }
    componentDidMount(){
        this.interval = setInterval(() => this.setState({ time: Date.now() }), 1000);


        this.setState({
            clock: new Clock({
                ...fischer,
                updateInterval,
                callback: async (state) => {
                    if (state.status === "done") {
                        if (this.state.turnOf && this.state.isWhite) {
                            console.log("black wins")
                            await this.state.gameConnection.invoke('LoseGame');
                        }
                        else if (!this.state.turnOf && !this.state.isWhite) {
                            console.log("white wins")
                            await this.state.gameConnection.invoke('LoseGame');
                        }
                    }
                }
            })
        })
        console.log("cdm called")
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
                            await this.state.gameConnection.invoke('LoseGame');
                        }
                        else if (!this.state.turnOf && !this.state.isWhite) {
                            console.log("white wins")
                            await this.state.gameConnection.invoke('LoseGame');
                        }
                    }
                }
            })
        })
        this.forceUpdate();
    }

    
    renderJoin() {
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
    onJoinGame = async (e) => {
        e.preventDefault()
        if (!this.state.joined) {
            this.setState({ joined: false })
            await this.state.gameConnection.invoke('EnterGame', this.state.gameType)
        }

    }
    onResign = async () => {
        await this.state.gameConnection.invoke('LoseGame');
    }
}
