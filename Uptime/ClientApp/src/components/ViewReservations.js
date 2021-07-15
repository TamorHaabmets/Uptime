import React, { Component } from 'react';


class ViewReservations extends Component {
    static displayName = ViewReservations.name;

    constructor(props) {
        super(props);
        this.state = { reservations: [], loading: true };

        fetch('api/reservations')
            .then(response => response.json())
            .then(data => {
                this.setState({ reservations: data, loading: false });
            });
    }

    static renderFilesTable(reservations) {
        return (
            <div className="container-fluid">
                <table className='table table-striped'>
                    <thead>
                        <tr>
                            <th>First name</th>
                            <th>Last name</th>
                            <th>Total price</th>
                            <th>Travel Route</th>
                            <th>Total travel time</th>
                            <th>Companies</th>
                            <th></th>
                        </tr>
                    </thead>
                    <tbody>
                        {reservations.map(reservation =>
                            <tr key={reservation.id}>
                                <td>{reservation.firstName}</td>
                                <td>{reservation.lastName}</td>
                                <td>{reservation.totalPrice}</td>
                                <td>{reservation.travelRoute.map(destination => <text>{destination} </text>)}</td>
                                <td>{reservation.totalTravelTime.days} days {reservation.totalTravelTime.hours} hours</td>
                                <td>{reservation.companies.map(company => <p>{company}</p>)}</td>
                            </tr>
                        )}
                    </tbody>
                </table>
            </div>
        );
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : ViewReservations.renderFilesTable(this.state.reservations);

        return (
            <div>
                <h1>Reservations</h1>
                <p>Here are all made reservations.</p>
                {contents}
            </div>
        );
    }
}

export default ViewReservations;
