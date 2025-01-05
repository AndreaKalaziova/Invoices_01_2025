import React from "react";
import priceFormator from "../utils/priceFormator";

//returns statistics for all invoices in repository; all-tim-sum and sum for actual year
const InvoiceStatistics = ({ items }) => {
    return (
        <div>
            <table className="table table-borderless">
                <thead>
                    <tr>
                        <th>Bilance za celé období:</th>
                        <th>Bilance za tento rok:</th>
                    </tr>
                </thead>
                <tbody>
                    {items ? (
                        <tr>
                            <td>{priceFormator(items.allTimeSum)}</td>
                            <td>{priceFormator(items.currentYearSum)}</td>
                        </tr>
                    ) : (
                        <tr>
                            <td colSpan="3">Žádná data k dispozici</td>
                        </tr>
                    )}
                </tbody>
            </table>
        </div>
    );
};

export default InvoiceStatistics;
