import React from 'react';
import {BoardComponent} from './BoardComponent';

import './styles.css';


export class ChessBoard extends BoardComponent {
    static displayName = ChessBoard.name;

    constructor(props) {
        super(props);
        console.log("I'm not a useless constructor, I am just a derived class!")
             
    }
    

    componentDidMount() {
        this.setState({gameType: 0})  
        super.componentDidMount()
        
        
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
                        <th>White moves</th>
                        <th>Black moves</th>
                    </thead>
                    <tbody>
                        {rows}
                    </tbody>
                </table>

            </div>)
    }
    moveToString(move) {
        if (move.piece === 0 && move.targetY - move.initialY === 2) {
            if (move.isCheck) return "O-O+"
            return "O-O";
        }
        else if (move.piece === 0 && move.initialY - move.targetY === 2) {
            if (move.isCheck) return "O-O-O+"
            return "O-O-O";
        }

        let initial = "";
        switch (move.piece) {
            case 0: initial += "K"; break;
            case 1: initial += "Q"; break;
            case 2: initial += "N"; break;
            case 3: initial += "B"; break;
            case 4: initial += "R"; break;
            default: break;
        }
        initial += this.colToChar(move.initialY);

        initial += (move.initialX + 1);

        if (move.isCapture) initial += "x";

        initial += this.colToChar(move.targetY)

        initial += (move.targetX + 1);


        switch (move.promoteTo) {
            case 1: initial += "=Q"; break;
            case 4: initial += "=R"; break;
            case 3: initial += "=B"; break;
            case 2: initial += "=N"; break;
            default: break;
        }
        if (move.isCheck) initial += "+";
        return initial;
    }
    colToChar(col) {
        switch (col) {
            case 0: return "a";
            case 1: return "b";
            case 2: return "c";
            case 3: return "d";
            case 4: return "e";
            case 5: return "f";
            case 6: return "g";
            case 7: return "h";
            default: break;
        }
    }

    renderBlack() {
        let whiteminutes = Math.floor(this.state.clock.state.remainingTime[0] / 60000).toLocaleString('en-US', { minimumIntegerDigits: 2, useGrouping: false });
        let blackminutes = Math.floor(this.state.clock.state.remainingTime[1] / 60000).toLocaleString('en-US', { minimumIntegerDigits: 2, useGrouping: false });
        let whiteseconds = Math.floor((this.state.clock.state.remainingTime[0] % 60000) / 1000).toLocaleString('en-US', { minimumIntegerDigits: 2, useGrouping: false });
        let blackseconds = Math.floor((this.state.clock.state.remainingTime[1] % 60000) / 1000).toLocaleString('en-US', { minimumIntegerDigits: 2, useGrouping: false });
        const rows = [];
        for (let i = 0; i < 8; i++) {

            const cols = [];
            for (let j = 0; j < 8; j++) {
                var ishighlighted = false;

                for (let p = 0; p < this.state.possibleMoves.length; p++) {
                    if (this.state.possibleMoves[p].targetX === i && this.state.possibleMoves[p].targetY === j) {
                        ishighlighted = true;
                    }
                }
                if (ishighlighted && this.state.turnOf === this.state.isWhite && this.state.duringMove) {
                    cols.push(<td><button className='chess' style={this.returnColor((i + j) % 2 === 0, true)} onClick={() => this.onClick(i, j)}>{this.returnText(i, j)}&#xFE0E;</button></td>);
                }
                else {
                    cols.push(<td><button className='chess' style={this.returnColor((i + j) % 2 === 0, false)} onClick={() => this.onClick(i, j)}>{this.returnText(i, j)}&#xFE0E;</button></td>);
                }
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
                    <strong>{this.state.whitepoints > this.state.blackpoints ? ("+") + (this.state.whitepoints - this.state.blackpoints) : ""}</strong>
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
                    <strong>{this.state.whitepoints < this.state.blackpoints ? ("+") + (this.state.blackpoints - this.state.whitepoints) : ""}</strong>
                    <div className='flexing resign-button'><button className='btn btn-secondary' onClick={() => this.onResign()}>Resign</button></div>
                </div>
            </div>)


    }
    renderWhite() {
        let whiteminutes = Math.floor(this.state.clock.state.remainingTime[0] / 60000).toLocaleString('en-US', { minimumIntegerDigits: 2, useGrouping: false });
        let blackminutes = Math.floor(this.state.clock.state.remainingTime[1] / 60000).toLocaleString('en-US', { minimumIntegerDigits: 2, useGrouping: false });
        let whiteseconds = Math.floor((this.state.clock.state.remainingTime[0] % 60000) / 1000).toLocaleString('en-US', { minimumIntegerDigits: 2, useGrouping: false });
        let blackseconds = Math.floor((this.state.clock.state.remainingTime[1] % 60000) / 1000).toLocaleString('en-US', { minimumIntegerDigits: 2, useGrouping: false });
        const rows = [];
        for (let i = 7; i >= 0; i--) {

            const cols = [];
            for (let j = 0; j < 8; j++) {
                var ishighlighted = false;

                for (let p = 0; p < this.state.possibleMoves.length; p++) {
                    if (this.state.possibleMoves[p].targetX === i && this.state.possibleMoves[p].targetY === j) {
                        ishighlighted = true;
                    }
                }
                if (ishighlighted && this.state.turnOf === this.state.isWhite && this.state.duringMove) {
                    cols.push(<td><button className='chess' style={this.returnColor((i + j) % 2 === 0, true)} onClick={() => this.onClick(i, j)}>{this.returnText(i, j)}&#xFE0E;</button></td>);
                }
                else {
                    cols.push(<td><button className='chess' style={this.returnColor((i + j) % 2 === 0, false)} onClick={() => this.onClick(i, j)}>{this.returnText(i, j)}&#xFE0E;</button></td>);
                }
            }
            rows.push(<tr>{cols}</tr>)
        }
        return (
            <div>
                <div className='flexing info-bar'>
                    <div className='clock-container'>
                        <strong>{blackminutes}:{blackseconds}</strong>
                    </div>
                    <strong>{this.state.otheruser}</strong>
                    <strong>{this.state.whitepoints < this.state.blackpoints ? ("+") + (this.state.blackpoints - this.state.whitepoints) : ""}</strong>
                </div>
                <table style={this.setBackground()} className='table' aria-labelledby="tabelLabel">
                    <tbody>
                        {rows}
                    </tbody>
                </table>
                <div className='flexing info-bar'>
                    <div className='clock-container'>
                        <strong>{whiteminutes}:{whiteseconds}</strong>
                    </div>
                    <strong>{this.state.ownuser}</strong>
                    <strong>{this.state.whitepoints > this.state.blackpoints ? ("+") + (this.state.whitepoints - this.state.blackpoints) : ""}</strong>
                    <div className='flexing resign-button'><button className='btn btn-secondary' onClick={() => this.onResign()}>Resign</button></div>
                </div>
            </div>)


    }

    renderPromoteSelection() {

        return (
            <div className="btn-group btn-group-toggle promote" role="group" aria-label="Basic radio toggle button group" onChange={this.handleChange} >
                <input type="radio" className="btn-check" value="Queen" name="promote" id="btnradio1" autoComplete='off' />
                <label className="btn btn-outline-dark" htmlFor="btnradio1">Queen</label>

                <input type="radio" className="btn-check" value="Knight" name="promote" id="btnradio2" autoComplete='off' />
                <label className="btn btn-outline-dark" htmlFor="btnradio2">Knight</label>

                <input type="radio" className="btn-check" value="Bishop" name="promote" id="btnradio3" autoComplete='off' />
                <label className="btn btn-outline-dark" htmlFor="btnradio3">Bishop</label>

                <input type="radio" className="btn-check" value="Rook" name="promote" id="btnradio4" autoComplete='off' />
                <label className="btn btn-outline-dark" htmlFor="btnradio4">Rook</label>
            </div>
        );
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

    handleChange = (e) => {
        switch (e.target.value) {
            case "Queen":
                this.setState({ promoteTo: 1 })
                break;
            case "Knight":
                this.setState({ promoteTo: 2 })
                break;
            case "Bishop":
                this.setState({ promoteTo: 3 })
                break;
            case "Rook":
                this.setState({ promoteTo: 4 })
                break;
            default:
                break;
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
        if (this.state.board[8 * x + y].piece == null) return ""
        switch (this.state.board[8 * x + y].piece.pieceName) {
            case 0:
                return this.state.board[8 * x + y].piece.isWhite ? "♔" : "♚"
            case 1:
                return this.state.board[8 * x + y].piece.isWhite ? "♕" : "♛"
            case 2:
                return this.state.board[8 * x + y].piece.isWhite ? "♘" : "♞"
            case 3:
                return this.state.board[8 * x + y].piece.isWhite ? "♗" : "♝"
            case 4:
                return this.state.board[8 * x + y].piece.isWhite ? "♖" : "♜"
            case 5:
                return this.state.board[8 * x + y].piece.isWhite ? "♙" : "♟"
            default:
                break;
        }
    }

    render() {

        let join = this.state.started ? <div></div> : this.renderJoin()
        let content = <div></div>
        if (this.state.isWhite) {
            content = this.state.loading ? <div></div> : this.renderWhite()
        }
        else {
            content = this.state.loading ? <div></div> : this.renderBlack()
        }
        let promote = !this.state.promotionVisible ? <div></div> : this.renderPromoteSelection()
        let win = <div></div>

        switch (this.state.winner) {
            case 0:
                win = <div className='end-screen'>
                    <p>White wins!</p>
                    <button className='btn btn-secondary' onClick={this.restart}>OK</button>
                </div>
                break;
            case 1:
                win = <div className='end-screen'>
                    <p>Black wins!</p>
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
                    {promote}
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
        if (this.state.board[8 * x + y].piece == null && !this.state.duringMove) return
        if (this.state.turnOf !== this.state.isWhite) return
        if (!this.state.duringMove) {
            this.setState({ duringMove: true, prevx: x, prevy: y })
            if (this.state.isWhite && x === 6 && this.state.board[8 * x + y].piece.pieceName === 5 && this.state.board[8 * x + y].piece.isWhite) this.setState({ promotionVisible: true })
            else if (!this.state.isWhite && x === 1 && this.state.board[8 * x + y].piece.pieceName === 5 && !this.state.board[8 * x + y].piece.isWhite) this.setState({ promotionVisible: true })
            await this.state.gameConnection.invoke('PossibleMoves', x, y);
        }
        else {
            this.setState({ duringMove: false, possibleMoves: [], promotionVisible: false });
            await this.state.gameConnection.invoke('MakeMove', JSON.stringify({initialX: this.state.prevx, 
                                                                    initialY: this.state.prevy,
                                                                    targetX: x,
                                                                    targetY: y,
                                                                    promoteTo: this.state.promoteTo}), 0);
        }
    }
  
   
    
}