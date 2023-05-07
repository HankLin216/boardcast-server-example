import React from 'react';
import ReactDOM from 'react-dom/client';
import DeviceTable from './Components/DeviceTable';
import RealTimeDeviceTable from './Components/RealTimeDeviceTable';
import Typography from '@mui/material/Typography';

const App = (): JSX.Element => {
  return (
    <div>
      <Typography variant="h5">Timer Table</Typography>
      <DeviceTable></DeviceTable>
      <Typography variant="h5">Real Time Table</Typography>
      <RealTimeDeviceTable></RealTimeDeviceTable>
    </div>
  );
};

const root = ReactDOM.createRoot(
  document.getElementById('root') as HTMLInputElement
);
root.render(
  <React.StrictMode>
    <App />
  </React.StrictMode>
);
