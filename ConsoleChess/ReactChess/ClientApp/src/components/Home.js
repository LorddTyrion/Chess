import React, { Component } from 'react';
import chess from './chess.png'



export class Home extends Component {
  static displayName = Home.name;

  render () {
    return (
      <div>
        <h1>Chess</h1>
        <img src={chess} alt=''></img>
        <p>Chess, one of the oldest and most popular board games, played by two opponents on a checkered board with specially designed pieces of contrasting colours, 
          commonly white and black. White moves first, after which the players alternate turns in accordance with fixed rules, each player attempting to force the opponent’s 
          principal piece, the King, into checkmate—a position where it is unable to avoid capture.</p>        
      </div>
    );
  }
}
