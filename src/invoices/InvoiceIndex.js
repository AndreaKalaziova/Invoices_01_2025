import React, { useEffect, useState } from "react";
import { apiDelete, apiGet } from "../utils/api";
import FlashMessage from "../components/FlashMessage";

import InvoiceTable from "./InvoiceTable";
import InvoiceFilter from "./InvoiceFilter";
import InvoiceStatistics from "../statistics/InvoiceStatistics";

/* InvoiceIndex displays InvoiceTable component - list of Invoices, fetches data from server*/
const InvoiceIndex = () => {
    //const [invoices, setInvoices] = useState([]);
    const [isPersonsLoading, setIsPersonsLoading] = useState(true);
    const [isInvoicesLoading, setIsInvoicesLoading] = useState(true);
    const [isStatisticsLoading, setIsStatisticsLoading] = useState(true);
    const [isLoading, setIsLoading] = useState(true);    //When isLoading is true, a <p>Nacitavam...</p> message is displayed instead of the componnets
    const [persons, setPersons] = useState([])
    const [invoiceStatistics, setInvoiceStatistics] = useState([]);
    const [flashMessage, setFlashMessage] = useState(null); // Tracks flash message content
    const [flashTheme, setFlashTheme] = useState(""); // Tracks flash message theme (success or error)
    const [invoicesState, setInvoices] = useState([]);
    const [filterState, setFilter] = useState({
        sellerId: undefined,
        buyerId: undefined,
        product: undefined,
        minPrice: undefined,
        maxPrice: undefined,
        limit: undefined,
    });

    //divinding invoices and statistics from persons, for reloading only invoices and its statistics data
    //fetches the list of invoices from the server
    const refreshData = () => {
        apiGet("/api/invoices")
            .then((data) => {
                setInvoices(data);      //update the invoice state with fetched data
                setIsInvoicesLoading(false);    // set loading to false after data is fetched
            })
            .catch((error) => {
                console.error("Chyba při načítání faktury:", error);
                setFlashMessage("Chyba při načítání dat: " + error.message);
                setFlashTheme("danger");
                setIsInvoicesLoading(false); // Stop loading even if there's an error
            });
        apiGet("/api/invoices/statistics")
            .then((data) => {
                setInvoiceStatistics(data);     //update the invoice statisctics state with fetched data
                setIsStatisticsLoading(false);            // set loading to false after data is fetched
            })
            .catch((error) => {
                console.error("Chyba při načítání faktury:", error);
                setFlashMessage("Chyba při načítání dat: " + error.message);
                setFlashTheme("danger");
                setIsStatisticsLoading(false); // Stop loading even if there's an error
            });
    };

    //deletes invoice by Id with confimation prompt and flash message about success/failure
    const deleteInvoice = async (id) => {
        // Confirmation prompt
        const confirmDelete = window.confirm("Určitě chcete tuto fakturu smazat?");
        if (!confirmDelete) return; // Abort deletion if user cancels

        try {
            await apiDelete("/api/invoices/" + id); //make API call to delete the person
            refreshData();      //re-fetch invoices and statistics after invoice deletion
            //set success flash message
            setFlashMessage("Faktura byla úspěšně smazána.");
            setFlashTheme("success");

            //delay before updating the state to remove the person
            setTimeout(() => {
                setInvoices(invoicesState.filter((invoice) => invoice._id !== id)); // Remove the deleted person from the list
                setFlashMessage(null); // Clear flash message
            }, 1800); // Flash message visible for 1.8 seconds
        } catch (error) {
            console.log(error.message);
            // Set error flash message
            setFlashMessage("Chyba při mazání faktury: " + error.message);
            setFlashTheme("danger");

            // Clear flash message after 3 seconds
            setTimeout(() => setFlashMessage(null), 1800);
        }
    };

    // fetched person details for the invoices
    useEffect(() => {
        refreshData();              //re-fetch invoices and statistics after invoice deletion
        apiGet("/api/persons")
            .then((data) => {
                setPersons(data);       //update the person state with fetched data
                setIsPersonsLoading(false);    //set loading to false after data is fetched
            })
            .catch((error) => {
                console.error("Chyba při načítání osoby:", error);
                setFlashMessage("Chyba při načítání dat: " + error.message);
                setFlashTheme("danger");
                setIsPersonsLoading(false); // Stop loading even if there's an error
            });
    }, []);         // empty array = this runs only once 


    const handleChange = (e) => {
        console.log(e.target);

        //if empty value selected (in components we have defined as true/false/''), we set it as undefined
        // pokud vybereme prázdnou hodnotu (máme definováno jako true/false/'' v komponentách), nastavíme na undefined
        if (
            e.target.value === "false" ||
            e.target.value === "true" ||
            e.target.value === ""
        ) {
            setFilter((prevState) => {
                return { ...prevState, [e.target.name]: undefined };
            });
        } else {
            setFilter((prevState) => {
                return { ...prevState, [e.target.name]: e.target.value };
            });
        }
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        const data = await apiGet("api/invoices", filterState);
        setInvoices(data);
    };

    // for button to clear selected filters and reload the page with not-filtered data
    const handleResetFilters = () => {
        setFilter({
            sellerId: "",
            buyerId: "",
            minPrice: "",
            maxPrice: "",
            product: "",
            limit: "",
        });
        window.location.reload(); // Reload the page
    };

    return (
        <div>
            <h1>Seznam faktur</h1>
            {/* Flash Message */}
            {flashMessage && <FlashMessage theme={flashTheme} text={flashMessage} />}

            {/* Loading states for each section */}
            {isInvoicesLoading ? (
                <h2>Načítávám Faktury...</h2>
            ) : isStatisticsLoading ? (
                <h2>Načítávám Statistiky...</h2>
            ) : isPersonsLoading ? (
                <h2>Načítávám Osoby...</h2>
            ) : (
                <>
                    <InvoiceFilter
                        handleChange={handleChange}
                        handleSubmit={handleSubmit}
                        handleReset={handleResetFilters} // Pass the reset function
                        persons={persons}
                        filter={filterState}
                        confirm="Filtrovat faktury"
                    />
                    <hr />
                    <InvoiceStatistics
                        items={invoiceStatistics}
                    />
                    <hr />
                    <InvoiceTable
                        deleteInvoice={deleteInvoice}
                        items={invoicesState}
                        label="Počet faktur:"
                    />
                </>
            )}
        </div>
    );
};
export default InvoiceIndex;