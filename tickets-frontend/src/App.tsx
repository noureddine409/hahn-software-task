import React, {useCallback, useEffect, useState} from 'react';
import {
    Box,
    Button,
    Chip,
    IconButton,
    InputAdornment,
    Table,
    TableBody,
    TableCell,
    TableContainer,
    TableHead,
    TableRow,
    TableSortLabel,
    TextField,
    Toolbar,
    Typography,
} from '@mui/material';
import {Delete, Edit} from '@mui/icons-material';
import {Ticket, TicketDto} from './models/types';
import AddTicketForm from "./components/AddTicketForm";
import EditTicketForm from "./components/EditTicketForm";
import TicketService from "./services/TicketService";
import Pagination from "./components/Pagination";

const PAGE_SIZE = 5;

const App: React.FC = () => {
    const [tickets, setTickets] = useState<Ticket[]>([]);
    const [openAdd, setOpenAdd] = useState(false);
    const [openEdit, setOpenEdit] = useState(false);
    const [formData, setFormData] = useState<Ticket | null>(null);
    const [search, setSearch] = useState('');
    const [sortDirection, setSortDirection] = useState<'asc' | 'desc'>('asc');
    const [totalCount, setTotalCount] = useState(1000);
    const [currentPage, setCurrentPage] = useState(1);
    const [loading, setLoading] = useState(false); // Loading state

    const fetchTickets = useCallback(async () => {
        setLoading(true);
        try {
            const allTickets = await TicketService.search({
                page: currentPage,
                size: PAGE_SIZE,
                sortDirection,
                keyword: search
            });
            setTickets(allTickets.data);
        } catch (error) {
            console.error('Failed to fetch tickets:', error);
        } finally {
            setLoading(false);
        }
    }, [currentPage, search, sortDirection]);

    useEffect(() => {
        fetchTickets();
    }, [fetchTickets]);

    const handleOpenAdd = () => setOpenAdd(true);
    const handleCloseAdd = () => setOpenAdd(false);

    const handleOpenEdit = (ticket: Ticket) => {
        setFormData(ticket);
        setOpenEdit(true);
    };

    const handleCloseEdit = () => {
        setFormData(null);
        setOpenEdit(false);
    };

    const handleAddSubmit = async (data: Ticket) => {
        const ticket: TicketDto = {status: data.status, description: data.description};
        try {
            const response = await TicketService.save(ticket);
            setTickets(prevTickets => [...prevTickets, response.data]);
            handleCloseAdd();
        } catch (error) {
            console.error('Error saving ticket:', error);
        }
    };

    const handleEditSubmit = async (data: Ticket) => {
        const updates: TicketDto = {description: data.description, status: data.status};
        try {
            const response = await TicketService.update(data.id, updates);
            setTickets(prevTickets =>
                prevTickets.map(ticket => ticket.id === response.data.id ? response.data : ticket)
            );
            handleCloseEdit();
        } catch (error) {
            console.error('Error saving ticket:', error);
        }
    };

    const handleDelete = async (id: number) => {
        try {
            await TicketService.delete(id);
            setTickets(prev => prev.filter(ticket => ticket.id !== id));
        } catch (error) {
            console.error('Error deleting ticket:', error);
        }
    };

    const handleSearchChange = (e: React.ChangeEvent<HTMLInputElement>) => setSearch(e.target.value);

    const handleSort = () => {
        setSortDirection(prev => (prev === 'asc' ? 'desc' : 'asc'));
    };

    const handlePageChange = (event: React.ChangeEvent<unknown>, value: number) => {
        setCurrentPage(value);
    };

    return (
        <Box sx={{width: '100%', padding: 2}}>
            <Toolbar>
                <Typography variant="h4" sx={{flexGrow: 1}}>
                    Ticket Management
                </Typography>
                <TextField
                    placeholder="Search..."
                    value={search}
                    onChange={handleSearchChange}
                    InputProps={{
                        startAdornment: <InputAdornment position="start">🔍</InputAdornment>,
                    }}
                    variant="outlined"
                    size="small"
                    sx={{marginRight: 2}}
                />
                <Button variant="contained" onClick={handleOpenAdd}>
                    Add Ticket
                </Button>
            </Toolbar>

            {loading ? ( // Loading state feedback
                <Typography variant="body1" color="textSecondary">Loading tickets...</Typography>
            ) : (
                <TableContainer>
                    <Table>
                        <TableHead>
                            <TableRow>
                                <TableCell>ID</TableCell>
                                <TableCell>Description</TableCell>
                                <TableCell>Status</TableCell>
                                <TableCell>
                                    <TableSortLabel
                                        active
                                        direction={sortDirection}
                                        onClick={handleSort}
                                    >
                                        Date
                                    </TableSortLabel>
                                </TableCell>
                                <TableCell>Actions</TableCell>
                            </TableRow>
                        </TableHead>
                        <TableBody>
                            {tickets.length === 0 ? (
                                <TableRow>
                                    <TableCell colSpan={5} align="center">
                                        <Typography variant="body1" color="textSecondary">
                                            No tickets available.
                                        </Typography>
                                    </TableCell>
                                </TableRow>
                            ) : (
                                tickets.map(ticket => (
                                    <TableRow key={ticket.id}>
                                        <TableCell>{ticket.id}</TableCell>
                                        <TableCell>{ticket.description}</TableCell>
                                        <TableCell>
                                            <Chip
                                                label={ticket.status}
                                                color={ticket.status === 'Open' ? 'success' : 'error'}
                                                variant="outlined"
                                            />
                                        </TableCell>
                                        <TableCell>{new Date(ticket.createdAt).toLocaleDateString()}</TableCell>
                                        <TableCell>
                                            <IconButton onClick={() => handleOpenEdit(ticket)}>
                                                <Edit/>
                                            </IconButton>
                                            <IconButton onClick={() => handleDelete(ticket.id)}>
                                                <Delete/>
                                            </IconButton>
                                        </TableCell>
                                    </TableRow>
                                ))
                            )}
                        </TableBody>
                    </Table>
                </TableContainer>
            )}

            <Pagination
                count={Math.ceil(totalCount / PAGE_SIZE)}
                page={currentPage}
                onPageChange={handlePageChange}
            />

            <AddTicketForm open={openAdd} onClose={handleCloseAdd} onSubmit={handleAddSubmit}/>
            {formData && (
                <EditTicketForm
                    open={openEdit}
                    onClose={handleCloseEdit}
                    onSubmit={handleEditSubmit}
                    initialData={formData}
                />
            )}
        </Box>
    );
};

export default App;
