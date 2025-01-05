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

import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { apiGet } from "../utils/api";
import Country from "./Country";

import InvoicesBySeller from "../statistics/InvoicesBySeller";
import InvoicesByBuyer from "../statistics/InvoicesByBuyer";


//PersonDetail componnet fetches and displays details of the selected person, also its invoices issued as seller and invoices recieved as buyer
const PersonDetail = () => {
    // extracting the 'id' parameter from the URL using useParams hooks
    const { id } = useParams();
    const [person, setPerson] = useState({});
    const [isLoading, setIsLoading] = useState(true);

    // fetch person details per provided 'id'
    useEffect(() => {
        apiGet("/api/persons/" + id)      //linking to API
            .then((data) => {
                setPerson(data);        // update person state with fetched data
                setIsLoading(false);    // set loading to false after data is fetched
            })
            .catch((error) => {
                console.error("Chyba při načítání osoby:", error);
                setFlashMessage("Chyba při načítání dat: " + error.message);
                setFlashTheme("danger");
                setIsLoading(false); // Stop loading even if there's an error
            });
    }, [id]);   // runs each time 'id' changes

    //get person's country name in the required language
    const country = Country.CZECHIA === person.country ? "Česká republika" : "Slovensko";

    const [invoicesBySeller, setInvoicesBySeller] = useState([]); // state for invoices where the perosn is Seller
    const [invoicesByBuyer, setInvoicesByBuyer] = useState([]); // state for invoices where the person is buyer

    // fetch invoices where the person is seller
    useEffect(() => {
        apiGet("/api/identification/" + person?.identificationNumber + "/sales")
            .then((data) => {
                setInvoicesBySeller(data);
            })                              // update person state with fetched data
            .catch((error) => {
                console.error(error);
            });
    }, [person]); // runs when 'person' state changes

    // fetch invoices where the person is buyer
    useEffect(() => {
        apiGet("/api/identification/" + person?.identificationNumber + "/purchases")
            .then((data) => {
                setInvoicesByBuyer(data);       // update person state with fetched data
            })
            .catch((error) => {
                console.error(error);
            });
    }, [person]);   // runs when 'person' state changes

    return (
        <>
            <div>
                <h1>Detail osoby</h1>
                <br />
                {/* Show loading message while fetching data */}
                {isLoading ? (
                    <h2>Načítávám...</h2>   //Once the data is loaded, the "Loading..." message disappears, and the PersonDetail component is displayed
                ) : (
                    <>
                        <hr />
                        {/*return person's details*/}
                        <h3>{person.name} ({person.identificationNumber})</h3>
                        <br />
                        <p>
                            <strong>DIČ:</strong>
                            <br />
                            {person.taxNumber}
                        </p>
                        <p>
                            <strong>Bankovní účet:</strong>
                            <br />
                            {person.accountNumber}/{person.bankCode} ({person.iban})
                        </p>
                        <p>
                            <strong>Tel.:</strong>
                            <br />
                            {person.telephone}
                        </p>
                        <p>
                            <strong>Mail:</strong>
                            <br />
                            {person.mail}
                        </p>
                        <p>
                            <strong>Sídlo:</strong>
                            <br />
                            {person.street}, {person.city},
                            {person.zip}, {country}
                        </p>
                        <p>
                            <strong>Poznámka:</strong>
                            <br />
                            {person.note}
                        </p>

                        {/*render person's invoices as seller*/}
                        <div>
                            <InvoicesBySeller
                                items={invoicesBySeller}
                                label="Počet vystavených faktur: "
                            />
                        </div>

                        {/*render person's invoices as buyer*/}
                        <div>
                            <InvoicesByBuyer
                                items={invoicesByBuyer}
                                label="Počet přijatých faktur: "
                            />
                        </div>
                    </>
                )}
            </div>
        </>
    );
};
export default PersonDetail;
