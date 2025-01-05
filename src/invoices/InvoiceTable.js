import React from "react";
import { Link } from "react-router-dom";
import priceFormator from "../utils/priceFormator";

//InvoiceTable renders table with Invoices from server with options to view details/edit/delete invoice
const InvoiceTable = ({ label, items, deleteInvoice }) => {
    return (
        <div>
            <p>
                {label} {items.length}
            </p>

            <table className="table table-bordered">
                <thead> {/*table header */}
                    <tr>
                        <th>#</th>          {/* ID */}
                        <th>Cislo</th>      {/* Invoice Number*/}
                        <th>Dodavatel</th>  {/* Name of Person/Seller*/}
                        <th>Odberatel</th>  {/* Name of Person/Buyer*/}
                        <th>Cena</th>       {/* Price on the Invoice*/}
                        <th>Produkt</th>    {/* Description of the product if the invoice*/}
                        <th colSpan={3}>Akce</th> {/* buttons for action View - Edit - Delete */}
                    </tr>
                </thead>
                <tbody>
                    {items.map((item, index) => (
                        <tr key={index + 1}>
                            <td>{index + 1}</td>
                            <td>{item.invoiceNumber}</td>
                            <td>
                                {/*person-seller name with link to its details*/}
                                <Link
                                    to={"/persons/show/" + item.seller?._id}
                                    style={{ textDecoration: "none", color: "blue" }}
                                >
                                    {item.seller?.name}
                                </Link>
                                <br /> <small> (ICO: {item.seller?.identificationNumber})</small>
                            </td>
                            <td>
                                {/*person-buyer name with link to its details*/}
                                <Link
                                    to={"/persons/show/" + item.buyer?._id}
                                    style={{ textDecoration: "none", color: "blue" }}
                                >
                                    {item.buyer?.name}
                                </Link>
                                <br /> <small> (ICO: {item.buyer?.identificationNumber})</small>
                            </td>
                            <td>{priceFormator(item.price)}</td>
                            <td>{item.product}</td>
                            <td>
                                <div className="btn-group">
                                    <Link
                                        to={"/invoices/show/" + item._id}
                                        className="btn btn-sm btn-info"
                                    >
                                        Zobrazit
                                    </Link>
                                    <Link
                                        to={"/invoices/edit/" + item._id}
                                        className="btn btn-sm btn-warning"
                                    >
                                        Upravit
                                    </Link>
                                    <button
                                        onClick={() => deleteInvoice(item._id)}
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
            {/* button navigates to Invoice Form for new invoice adding*/}
            <Link to={"/invoices/create"} className="btn btn-success">
                Nová faktura
            </Link>
        </div>
    );
};

export default InvoiceTable;
