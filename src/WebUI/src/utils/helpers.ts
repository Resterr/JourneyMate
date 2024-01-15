export const formatDate = (date: Date | string, timeZone: number): string => {
    if (!(date instanceof Date)) {
        date = new Date(date);
    }

    let updatedHour = date.getHours() + timeZone;

    if (updatedHour >= 24) {
        date.setDate(date.getDate() + 1);
        updatedHour %= 24;
    }

    const day: string = padZero(date.getDate());
    const month: string = padZero(date.getMonth() + 1); // Months are zero-based
    const year: number = date.getFullYear();
    const hour: string = padZero(updatedHour);
    const minutes: string = padZero(date.getMinutes());
    const formattedDate: string = `${day}/${month}/${year} ${hour}:${minutes}`;

    return formattedDate;
};

const padZero = (number: number): string => {
    return number < 10 ? "0" + number : number.toString();
};
