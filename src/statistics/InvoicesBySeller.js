import React from "react";
import priceFormator from "../utils/priceFormator";
import { Link } from "react-router-dom";

// component for invoices filtered out by pers's Identification Number, invoices issued (person is seller)
const InvoicesBySeller = ({ label, items }) => {
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
                            <td>{item.seller?.name}</td>
                            <td>
                                {/*person-buyer name with link to its details*/}
                                <Link
                                    to={"/persons/show/" + item.buyer?._id}
                                    style={{ textDecoration: "none", color: "blue" }}
                                >
                                    {item.buyer?.name}
                                </Link>
                            </td>
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

export default InvoicesBySeller;