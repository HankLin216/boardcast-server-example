/* eslint-disable @typescript-eslint/no-floating-promises */
import React, { useEffect, useState, useRef } from 'react';
import Table from '@mui/material/Table';
import TableBody from '@mui/material/TableBody';
import TableCell from '@mui/material/TableCell';
import TableContainer from '@mui/material/TableContainer';
import TableHead from '@mui/material/TableHead';
import TableRow from '@mui/material/TableRow';
import Paper from '@mui/material/Paper';
import Styles from './DevceTable.module.css';
import { keyframes } from '@mui/system';
import {
  type HubConnection,
  HubConnectionBuilder,
  LogLevel,
} from '@microsoft/signalr';

const refresh = keyframes`
    0% { color: red; }
    100% { color: inherit; }
`;

interface IDevice {
  id: string;
  name: string;
  type: string;
  status: string;
  update_Date: string;
  create_Date: string;
}

function RealTimeDeviceTable(): JSX.Element {
  const [conn, setConn] = useState<HubConnection>();
  const [devices, setDevices] = useState<IDevice[]>([]);
  const prevDeivces = useRef<IDevice[]>([]);

  useEffect(() => {
    // subscribe signalr
    const connection = new HubConnectionBuilder()
      .withUrl('http://localhost:5000/hubs/Device')
      .configureLogging(LogLevel.Information)
      .withAutomaticReconnect()
      .build();

    setConn(connection);

    // get device first
    async function GetAllDevices(): Promise<void> {
      try {
        const response = await fetch('http://localhost:5000/device');
        const devices = await response.json();
        setDevices(devices);
      } catch (err) {
        console.log(err);
      }
    }

    GetAllDevices();
  }, []);

  useEffect(() => {
    // start listening
    if (conn != null) {
      conn
        .start()
        .then(() => {
          console.log('Connected!');

          // update event
          conn?.on('UpdateDevice', (dev: IDevice) => {
            console.log(dev);

            const newDevices = prevDeivces.current.map((d) => {
              if (d.id === dev.id) {
                return {
                  ...d,
                  name: dev.name,
                  type: dev.type,
                  status: dev.status,
                  update_Date: dev.update_Date,
                };
              } else {
                return d;
              }
            });

            setDevices(newDevices);
          });
        })
        .catch((err) => {
          console.log(err);
        });
    }
  }, [conn]);

  useEffect(() => {
    prevDeivces.current = devices;
  }, [devices]);

  function sameAsPrevious(value: any, rowIdx: number, key: string): boolean {
    if (prevDeivces.current.length === 0) return false;

    return value === prevDeivces.current[rowIdx][key as keyof IDevice];
  }

  return (
    <TableContainer component={Paper}>
      <Table>
        <TableHead className={Styles.TableHeader}>
          <TableRow>
            <TableCell>ID</TableCell>
            <TableCell>Name</TableCell>
            <TableCell>Type</TableCell>
            <TableCell>Status</TableCell>
            <TableCell>Create Time</TableCell>
            <TableCell>Update Time</TableCell>
          </TableRow>
        </TableHead>
        <TableBody>
          {devices.map((d, rowIdx) => (
            <TableRow key={rowIdx}>
              {Object.keys(d).map((key) => {
                const currentValue = d[key as keyof IDevice];
                const isSameValue = sameAsPrevious(currentValue, rowIdx, key);

                return (
                  <TableCell
                    key={
                      !isSameValue
                        ? `${rowIdx}_${key}_${Math.random()}`
                        : `${rowIdx}_${key}`
                    }
                    sx={
                      !isSameValue
                        ? { animation: `${refresh} 3000ms;` }
                        : undefined
                    }
                    // className={!isSameValue ? tableClasses.refresh : undefined}
                  >
                    {d[key as keyof IDevice]}
                  </TableCell>
                );
              })}
            </TableRow>
          ))}
        </TableBody>
      </Table>
    </TableContainer>
  );
}

export default RealTimeDeviceTable;
