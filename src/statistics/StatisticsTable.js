import React from "react";
import { Link } from "react-router-dom";
import priceFormator from "../utils/priceFormator";

//get table with person name and its statistics - Revenue, Profit, Previous Year Turnover
const StatisticsTable = ({ label, items }) => {
    const lastYear = (new Date().getFullYear()) - 1; return (
        <div>
            <p>
                {label} {items.length}
            </p>

            <table className="table table-bordered">
                <thead>
                    <tr>
                        <th>#</th>
                        <th>Dodavatel</th>
                        <th>Celková částka přijatých plateb</th>
                        <th>Celková částka výnosu</th>
                        <th>Celkový obrat za rok {lastYear}</th>
                    </tr>
                </thead>
                <tbody>
                    {items.map((item, index) => (
                        <tr key={index + 1}>
                            <td>{index + 1}</td>
                            <td> {/*person name with link to its details*/}
                                {item.personId ? (
                                    <Link
                                        to={"/persons/show/" + item.personId}
                                        style={{ textDecoration: "none", color: "blue" }}
                                    >
                                        {item.personName}
                                    </Link>
                                ) : (
                                    item.personName || "Unknown"
                                )}
                            </td>
                            <td>{priceFormator(item.revenue)}</td>
                            <td>{priceFormator(item.profit)}</td>
                            <td>{priceFormator(item.previousYearTurnover)}</td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
};

export default StatisticsTable;
