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
import { apiDelete, apiGet } from "../utils/api";

import PersonTable from "./PersonTable";
import FlashMessage from "../components/FlashMessage";

/* PersonIndex displays PersonTable component - list of Persons, fetches data from server*/
const PersonIndex = () => {
    const [isLoading, setIsLoading] = useState(true);   //When isLoading is true, a <p>Nacitavam...</p> message is displayed instead of the PersonTable
    const [persons, setPersons] = useState([]);
    const [flashMessage, setFlashMessage] = useState(null); // Tracks flash message content
    const [flashTheme, setFlashTheme] = useState(""); // Tracks flash message theme (success or error)

    //fetches the list of person from the server
    useEffect(() => {
        apiGet("/api/persons")
            .then((data) => {
                setPersons(data);      //update the person state with fetched data
                setIsLoading(false);    // set loading to false after data is fetched
            })
            .catch((error) => {
                console.error("Chyba při načítání osoby:", error);
                setFlashMessage("Chyba při načítání dat: " + error.message);
                setFlashTheme("danger");
                setIsLoading(false); // Stop loading even if there's an error
            });
    }, []); // empty array = this runs only once 

    //deletes person by Id with confimation prompt and flash message about success/failure
    const deletePerson = async (id) => {
        // Confirmation prompt
        const confirmDelete = window.confirm("Určitě chcete tuto osobnost smazat?");
        if (!confirmDelete) return; // Abort deletion if user cancels

        try {
            await apiDelete("/api/persons/" + id);  //make API call to delete the person
            //set success flash message
            setFlashMessage("Osoba byla úspěšně smazána.");
            setFlashTheme("success");

            //delay before updating the state to remove the person
            setTimeout(() => {
                setPersons(persons.filter((item) => item._id !== id)); // Remove the deleted person from the list
                setFlashMessage(null); // Clear flash message
            }, 1800); // Flash message visible for 1.8 seconds
        } catch (error) {                           // retur error message
            console.log(error.message);
            // Set error flash message
            setFlashMessage("Chyba při mazání osoby: " + error.message);
            setFlashTheme("danger");

            // Clear flash message after 3 seconds
            setTimeout(() => setFlashMessage(null), 1800);
        }
    };

    //render PersonTable componnet with fetched data and delete handler, or a loading message
    return (
        <div>
            <h1>Seznam osob</h1>
            <br />
            {/* Flash Message */}
            {flashMessage && <FlashMessage theme={flashTheme} text={flashMessage} />}

            {/* Show loading message while fetching data */}
            {isLoading ? (
                <h2>Načítávám Osoby...</h2>   //Once the data is loaded, the "Loading..." message disappears, and the PersonTable component is displayed
            ) : (
                <PersonTable
                    deletePerson={deletePerson}
                    items={persons}
                    label="Počet osob:"
                />
            )}
        </div>
    );
};
export default PersonIndex;
