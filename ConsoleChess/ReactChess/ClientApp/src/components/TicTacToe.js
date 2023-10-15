import React from 'react';
//import { Clock } from 'chess-clock'
import './styles.css';
import {BoardComponent} from './BoardComponent';



   


export class TicTacToe extends BoardComponent {
    static displayName = TicTacToe.name;


    constructor(props) {
        super(props);
        console.log("I am also derived")
        //this.setState({gameType: 1})
        
    }

    componentDidMount() {
        this.setState({gameType: 1})
        super.componentDidMount();
        
        
    }
    componentWillUnmount() {
        clearInterval(this.interval);
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
                        <strong>{!this.state.isWhite ? whiteminutes:blackminutes}:{!this.state.isWhite ?whiteseconds: blackseconds}</strong>
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
                    <strong>{this.state.isWhite ? whiteminutes:blackminutes}:{this.state.isWhite ?whiteseconds: blackseconds}</strong>
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



    

    onClick = async (x, y) => {
        console.log(x + " " + y + " meg lett nyomva")
        if (this.state.board[3 * x + y].type !== 2 ) return
        if (this.state.turnOf !== this.state.isWhite) return       
        await this.state.gameConnection.invoke('MakeMove', JSON.stringify({X: x, Y: y}), 1);
        
    }
    
    
    
    
  
}