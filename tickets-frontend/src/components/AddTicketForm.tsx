import React from "react";
import {
    Button,
    Dialog,
    DialogActions,
    DialogContent,
    DialogTitle,
    FormControl,
    InputLabel,
    MenuItem,
    Select,
    TextField,
} from "@mui/material";
import { Ticket, TicketStatus } from "../models/types";

interface AddTicketFormProps {
    open: boolean;
    onClose: () => void;
    onSubmit: (data: Ticket) => void;
}

const AddTicketForm: React.FC<AddTicketFormProps> = ({ open, onClose, onSubmit }) => {
    const [description, setDescription] = React.useState('');
    const [status, setStatus] = React.useState<TicketStatus>('Open');
    const [error, setError] = React.useState<string | null>(null); // State for error messages
    const [touched, setTouched] = React.useState(false); // State to track if field has been touched

    const validateDescription = (value: string) => {
        if (value.trim().length < 3) {
            return 'Description must be at least 3 characters.';
        }
        if (value.trim().length > 40) {
            return 'Description must not exceed 40 characters.';
        }
        return null; // No error
    };

    const handleSubmit = () => {
        const validationError = validateDescription(description);
        if (validationError) {
            setError(validationError); // Set error message if validation fails
            setTouched(true); // Mark the field as touched to show error
            return; // Prevent form submission
        }

        const newTicket: Ticket = {
            id: Date.now(),
            description,
            status,
            createdAt: new Date().toDateString(),
        };
        onSubmit(newTicket);
        handleClose();
    };

    const handleClose = () => {
        setDescription('');
        setStatus('Open');
        setError(null); // Reset error message
        setTouched(false); // Reset touched state
        onClose();
    };

    return (
        <Dialog open={open} onClose={onClose}>
            <DialogTitle>Add Ticket</DialogTitle>
            <DialogContent>
                <TextField
                    autoFocus
                    margin="dense"
                    name="description"
                    label="Description"
                    type="text"
                    fullWidth
                    variant="outlined"
                    value={description}
                    onChange={(e) => {
                        setDescription(e.target.value);
                        const validationError = validateDescription(e.target.value);
                        if (validationError) {
                            setError(validationError); // Set error if validation fails
                        } else {
                            setError(null); // Clear error when description is valid
                        }
                    }}
                    onBlur={() => {
                        const validationError = validateDescription(description);
                        if (validationError) {
                            setError(validationError); // Show error when field loses focus
                            setTouched(true); // Mark as touched
                        }
                    }}
                    error={touched && !!error} // Highlight field if there's an error and it has been touched
                    helperText={touched && error} // Show error message if touched and invalid
                />
                <FormControl fullWidth variant="outlined" margin="dense">
                    <InputLabel>Status</InputLabel>
                    <Select
                        label="Status"
                        value={status}
                        onChange={(e) => setStatus(e.target.value as TicketStatus)} // Ensure correct type
                    >
                        <MenuItem value="Open">Open</MenuItem>
                        <MenuItem value="Closed">Closed</MenuItem>
                    </Select>
                </FormControl>
            </DialogContent>
            <DialogActions>
                <Button onClick={handleClose}>Cancel</Button>
                <Button onClick={handleSubmit} disabled={!!error}>Add</Button> {/* Disable if there is an error */}
            </DialogActions>
        </Dialog>
    );
};

export default AddTicketForm;
