import React from 'react';
import { Pagination as MuiPagination } from '@mui/material';

interface PaginationProps {
    count: number;
    page: number;
    onPageChange: (event: React.ChangeEvent<unknown>, value: number) => void;
}

const Pagination: React.FC<PaginationProps> = ({ count, page, onPageChange }) => {
    return (
        <MuiPagination
            count={count}
            page={page}
            onChange={onPageChange}
            color="primary"
            shape="rounded"
            sx={{ marginTop: 2, display: 'flex', justifyContent: 'center' }}
        />
    );
};

export default Pagination;
