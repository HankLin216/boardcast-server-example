import React from 'react';
import ReactDOM from 'react-dom/client';
import DeviceTable from './Components/DeviceTable';
import SignalRDeviceTable from './Components/SignalRDeviceTable';
import Typography from '@mui/material/Typography';

const App = (): JSX.Element => {
  return (
    <div>
      <Typography variant="h6">Timer Table</Typography>
      <Typography variant="subtitle1" color={'grey'}>
        refresh data by fetch wepapi every 10 seconds
      </Typography>
      <DeviceTable></DeviceTable>
      <br></br>
      <Typography variant="h6">SignalR Table</Typography>
      <Typography variant="subtitle1" color={'grey'}>
        Subscribe SignalR, update date when someone call the wepapi
      </Typography>
      <SignalRDeviceTable></SignalRDeviceTable>
    </div>
  );
};

const root = ReactDOM.createRoot(document.getElementById('root') as HTMLInputElement);
root.render(
  <React.StrictMode>
    <App />
  </React.StrictMode>
);
