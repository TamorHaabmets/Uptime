import React, { Component } from 'react';
import axios from 'axios';
import { Link } from 'react-router-dom';
import moment from 'moment';

class CreateReservation extends Component {

    constructor(props) {
        super(props);
        this.state = {
            firstName: '',
            lastName: '',
            nameEmpty: false,
            submitted: false,
            priceListIsValid: true
        };
    }

    handleSubmit = (e) => {
        e.preventDefault();
        var validUntil = new Date(this.props.location.validUntil);
        var validUntilMoment = moment(validUntil).utc(true);

        if (moment().isBefore(validUntilMoment)) {
            if (this.state.firstName === '' || this.state.lastName === '') {
                this.setState({
                    nameEmpty: true
                });
            }
            else {
                let body = {
                    firstName: this.state.firstName,
                    lastName: this.state.lastName,
                    totalPrice: this.props.location.totalPrice,
                    totalTravelTime: this.props.location.totalTime.ticks,
                    companies: this.props.location.companies,
                    travelRoute: this.props.location.travelRoutes
                };
                console.log(body);

                axios.post('/api/reservations', body)
                    .then(function (response) {
                        console.log(response);
                    })
                    .catch(function (error) {
                        console.log(error);
                    });
                this.setState({
                    nameEmpty: false,
                    submitted: true
                });
            }
        }
        else
        {
            this.setState({
                priceListIsValid: false
            });
        }
    }

    handleChange = (target) => {
        return (e) => {
            //debugger;
            this.setState({
                [target]: e.target.value,
            }, () => (
                console.log(this.state)))
        }
    }

    render() {
        const travelRoutes = this.props.location.travelRoutes;
        const companies = this.props.location.companies;

        let nameEmpty = this.state.nameEmpty
            ? <p>First name and Last name must not be empty.</p>
            : <text></text>;

        let submitted = this.state.submitted
            ? <p>Submitted! You can view your reservation here:</p>
            : <text></text>;

            let notValidPriceList = this.state.priceListIsValid
            ? <text></text>
            : <p>Oops! Prices have been expired. Tou have to start over from Interplanetary Flights in menu.</p>;
        return (
            <div>
                <form onSubmit={this.handleSubmit}>
                    <h1>Make reservation</h1>
                    <div className="form-group">
                        <label>First name:</label>
                        <input type="text" value={this.state.firstName} onChange={this.handleChange('firstName')} className="form-control form-control-md" />
                    </div>
                    <div className="form-group">
                        <label>Last name:</label>
                        <input type="text" value={this.state.lastName} onChange={this.handleChange('lastName')} className="form-control form-control-md" />
                    </div>
                    <div className="form-group">
                        <input className="btn" type="submit" value="Submit" />
                    </div>
                </form>
                {nameEmpty}
                {notValidPriceList}
                <h2>Reservation details</h2>
                <h3>Travel route: {travelRoutes.map(route => <text>{route} </text>)}</h3>
                <h3>Total price: {this.props.location.totalPrice}</h3>
                <h3>Total quoted travel time: {this.props.location.totalTime.days} days and {this.props.location.totalTime.hours} hours</h3>
                <h3>Total distance: {this.props.location.totalDistance}</h3>
                <h3>Start date: {this.props.location.travelStart}</h3>
                <h3>End date: {this.props.location.travelEnd}</h3>
                <h3>Companies: {companies.map(company => <text>{company}, </text>)}</h3>
                {submitted}
                <br />
                <Link classname='btn' role="button" to={{ pathname: '/ViewReservations' }}>View reservations</Link>
            </div>
        );
    }
}

export default CreateReservation;