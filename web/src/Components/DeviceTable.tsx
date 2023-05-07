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

function DeviceTable(): JSX.Element {
  const [devices, setDevices] = useState<IDevice[]>([]);
  const prevDeivces = useRef<IDevice[]>([]);

  useEffect(() => {
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

    const updateDeviceTimer = setInterval(() => {
      GetAllDevices();
    }, 10 * 1000);

    return () => {
      clearInterval(updateDeviceTimer);
    };
  }, []);

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
            <TableCell>CreateTime</TableCell>
            <TableCell>UpdateTime</TableCell>
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

export default DeviceTable;
