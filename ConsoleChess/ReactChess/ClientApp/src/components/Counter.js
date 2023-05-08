import React, { Component } from 'react';

import { Clock } from 'chess-clock'



const fischer = Clock.getConfig('Fischer Rapid 5|5')
const updateInterval = 1000
const callback = console.info

const clock = new Clock({
  ...fischer,
  updateInterval,
  callback,
})


export class Counter extends Component {
  static displayName = Counter.name;



  constructor(props) {
    super(props);
    this.state = { currentCount: 0, turnofwhite: true };
    this.incrementCounter = this.incrementCounter.bind(this);
  }

  

  incrementCounter() {

    this.setState({
      currentCount: this.state.currentCount + 1, turnofwhite: !this.state.turnofwhite
    });
    if(this.state.turnofwhite) clock.push(0);
    else clock.push(1);
    this.forceUpdate();
  }
  componentDidMount() {
    this.interval = setInterval(() => this.setState({ time: Date.now() }), 1000);
  }
  componentWillUnmount() {
    clearInterval(this.interval);
  }


  render() {
    console.log(clock.state);
    return (
      <div>
        <h1>Counter</h1>

        <p>This is a simple example of a React component.</p>

        <p aria-live="polite">Current count: <strong>{this.state.currentCount}</strong></p>
        <p aria-live="polite">White time: <strong>{clock.state.remainingTime[0]}</strong></p>
        <p aria-live="polite">Black time: <strong>{clock.state.remainingTime[1]}</strong></p>

        <button className="btn btn-primary" onClick={this.incrementCounter}>Increment</button>
        
      </div>
    );
  }
}
