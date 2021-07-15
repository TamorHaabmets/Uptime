import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import './custom.css'
import Routes from './components/Routes';
import CreateReservation from './components/CreateReservation';
import ViewReservations from './components/ViewReservations';

export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
      <Layout>
        <Route exact path='/' component={Routes} />
        <Route path='/ViewReservations' component={ViewReservations} />
        <Route path="/CreateReservation" component={CreateReservation} />
      </Layout>
    );
  }
}
