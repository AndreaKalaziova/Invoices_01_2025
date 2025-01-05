
// for Price formating into format "1 000,00 Kč"
export const priceFormator = (price) => {
    return Intl.NumberFormat("cs-CZ", {
        style: "currency",
        currency: "CZK",
        minimumFractionDigits: 2,
    }).format(price);
};

export default priceFormator;