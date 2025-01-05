import React from "react";
import { Link } from "react-router-dom";
import { priceFormator } from "../utils/priceFormator";

// component for invoices filtered out by pers's Identification Number, invoices recieved (person is buyer)
const InvoicesByBuyer = ({ label, items }) => {
    return (
        <div>
            <p>
                {label} {items.length}
            </p>
            <table className="table table-bordered">
                <thead>
                    <tr>
                        <th style={{ width: "4%" }}>#</th>
                        <th style={{ width: "24%" }}>Dodavatel</th>
                        <th style={{ width: "24%" }}>Odběratel</th>
                        <th style={{ width: "24%" }}>Částka</th>
                        <th style={{ width: "24%" }}>Číslo Faktury</th>
                    </tr>
                </thead>
                <tbody>
                    {items.map((item, index) => (
                        <tr key={index + 1}>
                            <td>{index + 1}</td>
                            <td>
                                {/*person-seller name with link to its details*/}
                                <Link
                                    to={"/persons/show/" + item.seller?._id}
                                    style={{ textDecoration: "none", color: "blue" }}
                                >
                                    {item.seller?.name}
                                </Link>
                            </td>
                            <td>{item.buyer?.name}</td>
                            <td>{priceFormator(item.price)}</td>
                            <td>
                                {/*invoice number with link to its details*/}
                                <Link
                                    to={"/invoices/show/" + item._id}
                                    style={{ textDecoration: "none", color: "blue" }}>
                                    {item.invoiceNumber}
                                </Link>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
};

export default InvoicesByBuyer;