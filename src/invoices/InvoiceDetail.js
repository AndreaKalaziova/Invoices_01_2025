import React, { useEffect, useState } from "react";
import { useParams, Link } from "react-router-dom";
import priceFormator from "../utils/priceFormator";

import { apiGet } from "../utils/api";
import dateStringFormatter from "../utils/dateStringFormatter";
import Country from "../persons/Country";

//InvoiceDetail componnet fetches and displays details of the selected invoice, with links to its seller and buyer details
const InvoiceDetail = () => {
    // extracting the 'id' parameter from the URL using useParams hooks
    const { id } = useParams();
    const [invoice, setInvoice] = useState({});
    const [isLoading, setIsLoading] = useState(true);

    // fetch invoice details per provided 'id'
    useEffect(() => {
        apiGet("/api/invoices/" + id)      //link to api
            .then((data) => {
                setInvoice({                // update invoice state with fetched data
                    invoiceNumber: data.invoiceNumber,
                    seller: data.seller,
                    sellerId: data.sellerId,
                    id: data.id,
                    buyer: data.buyer,
                    buyerId: data.buyerId,
                    issued: dateStringFormatter(data.issued, false),
                    dueDate: dateStringFormatter(data.dueDate, false),
                    product: data.product,
                    price: data.price,
                    vat: data.vat,
                    note: data.note,
                });
                setIsLoading(false);    // set loading to false after data is fetched
            })
            .catch((error) => {
                console.error("Chyba při načítání faktury:", error);
                setFlashMessage("Chyba při načítání dat: " + error.message);
                setFlashTheme("danger");
                setIsLoading(false); // Stop loading even if there's an error
            });
    }, [id]);                       // runs each time 'id' changes

    //get person's country name in the required language for the person details
    const country = Country.CZECHIA === invoice.country ? "Česká republika" : "Slovensko";

    return (
        <>
            <div>
                <h1>Detail faktrury</h1>
                <br />
                {/* Show loading message while fetching data */}
                {isLoading ? (
                    <h2>Načítávám...</h2>   //Once the data is loaded, the "Loading..." message disappears, and the PersonDetail component is displayed
                ) : (
                    <>
                        <hr />
                        {/*return invoice details*/}
                        <h3>{invoice.invoiceNumber}</h3>
                        <p>
                            <strong>Dodavatel:</strong>
                            <br />
                            <Link
                                to={"/persons/show/" + invoice.seller?._id}
                                style={{ textDecoration: "none", color: "blue" }}
                            >
                                {invoice.seller?.name}
                            </Link>
                            <br /> <small> (ICO: {invoice.seller?.identificationNumber})</small>
                        </p>
                        <p>
                            {/*link to buyer's details*/}
                            <strong>Odberatel:</strong>
                            <br />
                            <Link
                                to={"/persons/show/" + invoice.buyer?._id}
                                style={{ textDecoration: "none", color: "blue" }}
                            >
                                {invoice.buyer?.name}
                            </Link>
                            <br /> <small> (ICO: {invoice.buyer?.identificationNumber})</small>
                            {/* without the link:
                    {invoice.buyer?.name} <br />
                    <small> (ICO: {invoice.seller?.identificationNumber}, {invoice.seller?.city},{country})</small> */}
                        </p>
                        <p>
                            <strong>Datum vystaveni:</strong>
                            <br />
                            {invoice.issued}
                        </p>
                        <p>
                            <strong>Datum splatnosti:</strong>
                            <br />
                            {invoice.dueDate}
                        </p>
                        <p>
                            <strong>Produkt:</strong>
                            <br />
                            {invoice.product}
                        </p>
                        <p>
                            <strong>Cena:</strong>
                            <br />
                            {priceFormator(invoice.price)}
                        </p>
                        <p>
                            <strong>DPH:</strong>
                            <br />
                            {invoice.vat}%
                        </p>
                        <p>
                            <strong>Poznámka:</strong>
                            <br />
                            {invoice.note}
                        </p>

                    </>
                )}
            </div>
        </>
    );
};

export default InvoiceDetail;