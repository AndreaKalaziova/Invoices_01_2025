import React, { useEffect, useState } from "react";
import { apiGet } from "../utils/api";

import StatisticsTable from "./StatisticsTable";


//get table with person name and its statistics from StatisticsTable - Revenue, Profit, Previous Year Turnover
const StatisticsIndex = () => {
    const [isLoading, setIsLoading] = useState(true);
    const [statistics, setStatistics] = useState([]);

    useEffect(() => {
        apiGet("/api/persons/statistics")       //linking to API
            .then((data) => {
                setStatistics(data);                // update statistic state with fetched data
                setIsLoading(false);                // set loading to false after data is fetched
            })
            .catch((error) => {
                console.error("Chyba při načítání dat:", error);
                setFlashMessage("Chyba při načítání dat: " + error.message);
                setFlashTheme("danger");
                setIsLoading(false); // Stop loading even if there's an error
            })
    }, []);

    return (
        <div>
            <h1>Statistisky osob</h1>
            <br />
            {/* Show loading message while fetching data */}
            {isLoading ? (
                <h2>Načítávám Statistisky...</h2>   //Once the data is loaded, the "Loading..." message disappears, and the PersonDetail component is displayed
            ) : (
                <StatisticsTable
                    items={statistics}
                    label="Počet osob:"
                />
            )}
        </div>
    );
};
export default StatisticsIndex;
