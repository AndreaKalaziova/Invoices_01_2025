
import InputSelect from "../components/InputSelect";
import InputField from "../components/InputField"

// component for filtering out invoices 
const InvoiceFilter = (props) => {
    const handleChange = (e) => {
        props.handleChange(e);
    };

    // applying selected filters
    const handleSubmit = (e) => {
        props.handleSubmit(e);
    };

    //reseting the selected filters + reload the page with nont-filtered-out data
    const handleReset = (e) => {
        e.preventDefault();
        props.handleReset();    // Call the reset function provided by the parent
    };

    const filter = props.filter;

    return (
        <form onSubmit={handleSubmit}>
            <div className="row">
                <div className="col">
                    {/*drop-down menu for selecting of active/available sellers by name*/}
                    <InputSelect
                        name="sellerId"
                        items={props.persons}
                        handleChange={handleChange}
                        label="Dodavatel"
                        prompt="nevybrán"
                        value={
                            filter.sellerId
                        }
                    />
                </div>
                <div className="col">
                    {/*drop-down menu for selecting of active/available buyerss by name*/}
                    <InputSelect
                        name="buyerId"
                        items={props.persons}
                        handleChange={handleChange}
                        label="Odběratel"
                        prompt="nevybrán"
                        value={
                            filter.buyerId
                        }
                    />
                </div>
            </div>

            <div className="row">
                <div className="col">
                    {/*for filtering with this or higher price*/}
                    <InputField
                        type="number"
                        min="0"
                        name="minPrice"
                        handleChange={handleChange}
                        label="Cena od"
                        prompt="(neuvedeno)"
                        value={filter.minPrice ? filter.minPrice : ""} //value={filter.minPrice || ""}
                    />
                </div>
                <div className="col">
                    {/*for filtering with this or lower price*/}
                    <InputField
                        type="number"
                        min="0"
                        name="maxPrice"
                        handleChange={handleChange}
                        label="Cena do"
                        prompt="(neuvedeno)"
                        value={filter.maxPrice ? filter.maxPrice : ""}
                    />
                </div>

                <div className="col">
                    {/*for filtering by product*/}
                    <InputField
                        type="text"
                        name="product"
                        handleChange={handleChange}
                        label="Produkt"
                        prompt="(neuveden)"
                        value={filter.product} //  value={filter.product || ""}
                    />
                </div>

                <div className="col">
                    {/*limits of return findings*/}
                    <InputField
                        type="number"
                        min="1"
                        name="limit"
                        handleChange={handleChange}
                        label="Limit počtu faktur"
                        prompt="(neuvedeno)"
                        value={filter.limit ? filter.limit : ""}
                    />
                </div>
            </div>

            <div className="row">
                <div className="col">
                    {/*button for filtering by applied filters*/}
                    <input
                        type="submit"
                        label="Filtrovat faktury"
                        className="btn btn-success float-right mt-2"
                        value={props.confirm}
                    />
                </div>
                <div className="col">
                    {/*button for restting filters and reloading the page*/}
                    <button
                        type="button"
                        className="btn btn-success float-left mt-2"
                        onClick={handleReset}
                    >
                        Resetovat filtry
                    </button>
                </div>
            </div>
        </form>
    );
};

export default InvoiceFilter;
