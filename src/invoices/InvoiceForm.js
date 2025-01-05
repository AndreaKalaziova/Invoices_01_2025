import React, { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { apiGet, apiPost, apiPut } from "../utils/api";
import dateStringFormatter from "../utils/dateStringFormatter";

import InputField from "../components/InputField";
import InputSelect from "../components/InputSelect";
import FlashMessage from "../components/FlashMessage";

//InvoiceForm for adding new person (if no 'id' provided) or eding person's details (if 'id' is provided) 
const InvoiceForm = () => {
    const navigate = useNavigate();
    const { id } = useParams();     // get invoice ID from the URL params to determine if editing or creating
    const [invoice, setInvoice] = useState({
        invoiceNumber: "",
        seller: { _id: "" },
        buyer: { _id: "" },
        issued: "",
        dueDate: "",
        product: "",
        price: "",
        vat: "",
        note: ""
    });                                              //initial state for invoice details

    const [person, setPerson] = useState([]);       // for person associated with the invoice (seller / buyer) 
    const [persons, setPersons] = useState([]);     //list of all persons for dropdown selection
    const [isLoading, setIsLoading] = useState(true);
    const [sentState, setSent] = useState(false);   // tracks if the form submission was attempted
    const [successState, setSuccess] = useState(false); // racks if the submission was successful
    const [errorState, setError] = useState(null);      // tracks error messages
    const [validationErrors, setValidationErrors] = useState({}); // Track validation errors

    //fetch invoice details if 'id' present - for edit --- proc????
    useEffect(() => {
        if (id) {
            apiGet("/api/invoices/" + id)
                .then((data) => {
                    setInvoice(data);        // update invoice state with fetched data
                    setPerson(data);        // update person state with fetched data
                    setIsLoading(false);    // set loading to false after data is fetched
                })
                .catch((error) => {
                    console.error("Chyba při načítání detailu faktury:", error);
                    console.error("Chyba při načítání detailu osoby:", error);
                    setFlashMessage("Chyba při načítání dat: " + error.message);
                    setFlashTheme("danger");
                    setIsLoading(false); // Stop loading even if there's an error
                });
        } else {
            setIsLoading(false);    // No id means this is a create operation; stop loading immendiately
        }
        apiGet("/api/persons").then((data) => setPersons(data));
    }, [id]);       //runs when 'id' changes

    const validateForm = () => {
        const errors = {};

        // Check required fields
        if (!invoice.invoiceNumber) errors.invoiceNumber = "Číslo faktury je povinné.";
        if (!invoice.issued) errors.issued = "Datum vystavení je povinné.";
        if (!invoice.dueDate) errors.dueDate = "Datum splatnosti je povinné.";
        if (!invoice.product) errors.product = "Produkt je povinný.";
        if (!invoice.price) errors.price = "Cena je povinná.";
        if (!invoice.vat) errors.vat = "DPH je povinné.";

        // Validate seller field
        if (!invoice.seller || !invoice.seller._id) {
            errors.seller = "Pole 'Dodavatel' je povinné.";
        }
        // Validate buyer field
        if (!invoice.buyer || !invoice.buyer._id) {
            errors.buyer = "Pole 'Odberatel' je povinné.";
        }
        // Validate dates, date of issued must be earlier then due date
        if (invoice.issued && invoice.dueDate && new Date(invoice.issued) > new Date(invoice.dueDate))
            errors.dueDate = "Datum vystavení musí být dříve než datum splatnosti.";

        setValidationErrors(errors);
        return Object.keys(errors).length === 0;
    };

    // handles the form submision
    // send POST for creation or PUT for edit 
    const handleSubmit = (e) => {
        e.preventDefault();

        if (!validateForm()) return;    // Validate form and stop if there are errors

        // if id present? true = PUT/edit : false = POST/create
        (id ? apiPut("/api/invoices/" + id, invoice) : apiPost("/api/invoices", invoice))
            .then((data) => {
                setSent(true);
                setSuccess(true);
                setTimeout(() => {
                    navigate("/invoices");      //redirect to invoice list after timeout
                }, 1800);
            })
            .catch((error) => {
                console.log(error.message);
                setError(error.message);
                setSent(true);
                setSuccess(false);
            });
    };

    const sent = sentState;         // track submision state
    const success = successState;   // track success state

    return (
        <div>
            <h1>{id ? "Upravit" : "Vytvořit"} fakturu</h1>
            <hr />
            {errorState ? (
                <div className="alert alert-danger">{errorState}</div>
            ) : null}
            {sent && (
                <FlashMessage
                    theme={success ? "success" : ""}
                    text={success ? "Uložení faktury proběhlo úspěšně." : "Nastala chyba při uložení"}
                />
            )}
            {/* Show loading message while fetching data */}
            {isLoading ? (
                <h2>Načítávám...</h2>   //Once the data is loaded, the "Loading..." message disappears, and the InvoiceForm component is displayed
            ) : (
                <>
                    {/* Render form after data has been fetched */}
                    <form onSubmit={handleSubmit}>
                        <InputField
                            required={true}
                            type="number"
                            name="invoiceNumber"
                            min="01010101"
                            label="Cislo faktury"
                            prompt="Zadejte cislo faktury"
                            value={invoice.invoiceNumber}
                            handleChange={(e) => {
                                setInvoice({ ...invoice, invoiceNumber: e.target.value });
                            }}
                            disabled={!!id} // if id present = update, the filed is disabled
                            error={validationErrors.invoiceNumber}
                        />

                        <InputSelect
                            required={true}
                            name="seller"
                            label="Dodavatel"
                            prompt="Zadejte dodavatele"
                            items={persons}
                            value={invoice.seller._id}
                            allowEmptyInsert={!id} //if id not present = insert (if id present=edit) => allow empty field on load
                            handleChange={(e) => {
                                setInvoice({ ...invoice, seller: { _id: e.target.value } });
                            }}
                            error={validationErrors.seller}
                        />

                        <InputSelect
                            required={true}
                            name="buyer"
                            label="Odberatel"
                            prompt="Zadejte odberatele"
                            items={persons}
                            value={invoice.buyer._id}
                            allowEmptyInsert={!id} ///if id not present = insert (if id present=edit) => allow empty field on load
                            handleChange={(e) => {
                                setInvoice({ ...invoice, buyer: { _id: e.target.value } });
                            }}
                            error={validationErrors.buyer}
                        />

                        <InputField
                            required={true}
                            type="date"
                            name="issued"
                            min="2000-01-01"
                            label="Datum vystaveni"
                            prompt="Zadejte datum vystaveni:"
                            value={dateStringFormatter(invoice.issued)}
                            handleChange={(e) => {
                                setInvoice({ ...invoice, issued: e.target.value });
                            }}
                            error={validationErrors.issued}
                        />

                        <InputField
                            required={true}
                            type="date"
                            name="dueDate"
                            min="2000-01-01"
                            label="Datum splatnosti"
                            prompt="Zadejte datum splatnosti:"
                            value={dateStringFormatter(invoice.dueDate)}
                            handleChange={(e) => {
                                setInvoice({ ...invoice, dueDate: e.target.value });
                            }}
                            error={validationErrors.dueDate}
                        />

                        <InputField
                            required={true}
                            type="text"
                            name="product"
                            min="2"
                            max="250"
                            label="Produkt"
                            prompt="Zadejte produkt"
                            value={invoice.product}
                            handleChange={(e) => {
                                setInvoice({ ...invoice, product: e.target.value });
                            }}
                            error={validationErrors.product}
                        />

                        <InputField
                            required={true}
                            type="number"
                            name="price"
                            label="Cena (Kč)"
                            prompt="Zadejte cenu"
                            value={invoice.price}
                            handleChange={(e) => {
                                const value = parseFloat(e.target.value); // Ensure the value is a number
                                setInvoice({ ...invoice, price: value });   // set the parsed value, as e.target.value get string i/o number
                            }}
                            error={validationErrors.price}
                        />

                        <InputField
                            required={true}
                            type="number"
                            name="vat"
                            min="0"
                            max="100"
                            label="DPH (%)"
                            prompt="Zadejte sazbu DPH"
                            value={invoice.vat}
                            handleChange={(e) => {
                                setInvoice({ ...invoice, vat: e.target.value });
                            }}
                            error={validationErrors.vat}
                        />

                        <InputField
                            required={true}
                            type="text"
                            name="note"
                            min="2"
                            max="200"
                            label="Poznamka"
                            value={invoice.note}
                            handleChange={(e) => {
                                setInvoice({ ...invoice, note: e.target.value });
                            }}
                            error={validationErrors.note}
                        />
                        <input type="submit" className="btn btn-success" value="Uložit" />
                    </form>
                </>
            )}
        </div>
    );
};

export default InvoiceForm;