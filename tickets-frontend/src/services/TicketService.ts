import {SearchCriteria, Ticket, TicketDto} from "../models/types";
import {httpClient} from "../middleware/axiosConfig";

const TicketService = {

        search: async (searchCriteria: SearchCriteria) => {
            try {
                const {page, size, keyword, sortDirection} = searchCriteria;
                return await httpClient.get<Ticket[]>('/tickets', {
                    params: {
                        page,
                        size,
                        keyword,
                        sortDirection,
                    },
                });
            } catch (error) {
                console.error('Error searching for tickets:', error);
                throw error;
            }
        },

        delete:
            async (id: number) => {
                try {
                    return await httpClient.delete(`/tickets/${id}`);
                } catch (error) {
                    console.error(`Error deleting ticket with ID ${id}:`, error);
                    throw error;
                }
            },

        update:
            async (ticketId: number, ticket: TicketDto) => {
                try {
                    return await httpClient.put(`/tickets/${ticketId}`, ticket);
                } catch (error) {
                    console.error(`Error updating ticket with ID ${ticketId}:`, error);
                    throw error;
                }
            },

        save:
            async (ticket: TicketDto) => {
                try {
                    return await httpClient.post("/tickets", ticket);
                } catch (error) {
                    console.error('Error saving ticket:', error);
                    throw error;
                }
            }
    }
;

export default TicketService;
