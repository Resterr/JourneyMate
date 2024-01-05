export const formatDate = (date: Date | string, timeZone: number): string => {
    if (!(date instanceof Date)) {
        date = new Date(date);
    }

    const day: string = padZero(date.getDate());
    const month: string = padZero(date.getMonth() + 1); // Months are zero-based
    const year: number = date.getFullYear();
    const hours: string = padZero(
        date.getHours() + timeZone > 23 ? 0 : date.getHours() + timeZone,
    );
    const minutes: string = padZero(date.getMinutes());
    const formattedDate: string = `${day}/${month}/${year} ${hours}:${minutes}`;

    return formattedDate;
};

const padZero = (number: number): string => {
    return number < 10 ? "0" + number : number.toString();
};
