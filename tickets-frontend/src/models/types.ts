export type TicketStatus = 'Open' | 'Closed'; // Define allowed status values

export interface Ticket {
    id: number;
    description: string;
    status: TicketStatus;
    createdAt: string;
}

export interface TicketDto {
    description: string;
    status: TicketStatus;
}

export interface SearchCriteria {
    keyword: string;
    page: number;
    size: number;
    sortDirection: "asc" | "desc"
}


