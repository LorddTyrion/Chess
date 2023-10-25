import React, { Component } from 'react';

export const ProgressContext = React.createContext({
    progress: false,
    setProgress: () => {}
  });

export class ProgressContextProvider extends Component{
    setProgress = progress => {
        this.setState({ progress });
    };
    
    state = {
        progress: false,
        setProgress: this.setProgress
    };
    render(){
        return(
            <ProgressContext.Provider value={this.state}>
                {this.props.children}
            </ProgressContext.Provider>
        )
    }

}