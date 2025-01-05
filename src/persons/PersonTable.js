/*  _____ _______         _                      _
 * |_   _|__   __|       | |                    | |
 *   | |    | |_ __   ___| |___      _____  _ __| | __  ___ ____
 *   | |    | | '_ \ / _ \ __\ \ /\ / / _ \| '__| |/ / / __|_  /
 *  _| |_   | | | | |  __/ |_ \ V  V / (_) | |  |   < | (__ / /
 * |_____|  |_|_| |_|\___|\__| \_/\_/ \___/|_|  |_|\_(_)___/___|
 *                                _
 *              ___ ___ ___ _____|_|_ _ _____
 *             | . |  _| -_|     | | | |     |  LICENCE
 *             |  _|_| |___|_|_|_|_|___|_|_|_|
 *             |_|
 *
 *   PROGRAMOVÁNÍ  <>  DESIGN  <>  PRÁCE/PODNIKÁNÍ  <>  HW A SW
 *
 * Tento zdrojový kód je součástí výukových seriálů na
 * IT sociální síti WWW.ITNETWORK.CZ
 *
 * Kód spadá pod licenci prémiového obsahu a vznikl díky podpoře
 * našich členů. Je určen pouze pro osobní užití a nesmí být šířen.
 * Více informací na http://www.itnetwork.cz/licence
 */

import React from "react";
import { Link } from "react-router-dom";

//PersonTable renders table with Persons from server with options to view details/edit/delete person
const PersonTable = ({ label, items, deletePerson }) => {
    return (
        <div>
            <p>
                {label} {items.length}
            </p>

            <table className="table table-bordered">
                <thead> {/*table header */}
                    <tr>
                        <th>#</th>
                        <th>Jméno</th>
                        <th colSpan={3}>Akce</th>
                    </tr>
                </thead>
                <tbody> {/* table body */}
                    {items.map((item, index) => (
                        <tr key={index + 1}>
                            <td>{index + 1}</td> {/* ID */}
                            <td>{item.name}</td> {/* Name of Person*/}
                            <td> {/* buttons for action View - Edit - Delete */}
                                <div className="btn-group">
                                    <Link
                                        to={"/persons/show/" + item._id}
                                        className="btn btn-sm btn-info"
                                    >
                                        Zobrazit
                                    </Link>
                                    <Link
                                        to={"/persons/edit/" + item._id}
                                        className="btn btn-sm btn-warning"
                                    >
                                        Upravit
                                    </Link>
                                    <button
                                        onClick={() => deletePerson(item._id)}
                                        className="btn btn-sm btn-danger"
                                    >
                                        Odstranit
                                    </button>
                                </div>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>
            {/* button navigates to Person Form for new person adding*/}
            <Link to={"/persons/create"} className="btn btn-success">
                Nová osoba
            </Link>
        </div>
    );
};

export default PersonTable;
