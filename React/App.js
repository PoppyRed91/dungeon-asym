import { StatusBar } from 'expo-status-bar';
import { useEffect, useState } from 'react';
import { StyleSheet, Pressable, Text, View, Button } from 'react-native';

import { io } from 'socket.io-client';



export default function App() {

    const socket = io("http://192.168.50.3:3000", { transports: ["websocket"], autoConnect: false });

    useEffect(() => {
        console.log("effect");
        socket.on("connect", () => {
            console.log("Connected to server big success")
            socket.emit("userdata", "pepa")
        });

        socket.on("msg", (data) => {
            console.log("Received message:", data)
        });

        socket.on("disconnect", () => {
            console.log("Disconnected from server.");
        })

    }, [socket]);

    function connect() {
        socket.connect();
        console.log("connecting");
    }

    function disconnect() {
        socket.disconnect();
        console.log("disconnecting");
    }

    function sendMessage(event, message) {
        socket.emit(event, message);
    }

    return (
        <>
            <View style={styles.container}>
                <Text style={styles.text}>Navigate through dungeon</Text>
                <Pressable style={styles.button} onPress={() => connect()}><Text style={styles.buttonLabel}>Host</Text></Pressable>
                <Pressable style={styles.button} onPress={() => disconnect()}><Text style={styles.buttonLabel}>Disconnect</Text></Pressable>
            </View>
        </>
    );
}

const styles = StyleSheet.create({
    container: {
        flex: 1,
        backgroundColor: '#25292e',
        alignItems: 'center',
        justifyContent: 'center',
    },
    text: {
        color: 'lightsteelblue'
    },
    button: {
        borderRadius: 10,
        width: 128,
        height: 64,
        alignItems: 'center',
        justifyContent: 'center',
        flexDirection: 'row',
        backgroundColor: 'blue'
    },
    buttonLabel: {
        color: 'lightsteelblue',
        fontSize: 16,
    },
});