import React, { Component } from 'react';
import chess from './chess.png'
import tictactoe from './tictactoe.png'
import './styles.css';
import { Link } from 'react-router-dom';



export class Home extends Component {
  static displayName = Home.name;

  render () {
    return (
      <div>
        <h1>Board Game Framework</h1>
        <strong>This is the Board Game Framework. A place, where you can play various board games, and add your own favourites. A collaboration between developers and players.</strong>
        <div className='game-line'>
          <img className='game-image' src={chess} alt=''></img>
          <div className='play-container'><button className='btn btn-secondary play-button'><Link className='no-decor' to="/chessboard">Play chess</Link></button></div>     
        </div>
        <div className='game-line'>
          <img className='game-image' src={tictactoe} alt=''></img>
          <div className='play-container'><button className='btn btn-secondary play-button'><Link className='no-decor' to="/tictactoe">Play tic tac toe</Link></button></div>
        </div>
      </div>
    );
  }
}
