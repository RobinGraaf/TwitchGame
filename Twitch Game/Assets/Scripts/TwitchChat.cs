using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.ComponentModel;
using System.Net.Sockets;
using System.IO;
using UnityEngine.UI;

public class TwitchChat : MonoBehaviour {

    private TcpClient twitchClient;
    private StreamReader reader;
    private StreamWriter writer;

    string username, channelName, chatMessagePrefix; //Get the password from https://twitchapps.com/tmi
    string channel, password;
    DateTime lastMessageSendtime;

    public GameObject enemySpawner;
    public GameObject enemyPrefab;

    Queue<string> sendMessageQueue;

    private void Awake() {
        this.channel = GameManager.instance.GetUsername();
        this.password = GameManager.instance.GetPassword();
    }

    void Start () {
        sendMessageQueue = new Queue<string>();
        this.username = channel.ToLower();
        this.channelName = username;
        chatMessagePrefix = String.Format(":{0}!{0}@{0}.tmi.twitch.tv PRIVMSG #{1} :", username, channelName);

        Connect();
    }
    
    void SendTwitchMessage(string message) {
        //SendTwitchMessage(String.Format("Hello, {0}", speaker));
        sendMessageQueue.Enqueue(message);
    }

    void Update () {
        if (!twitchClient.Connected) {
            Connect();
        }

        TryReceivingMessages();
        TrySendingMessages();
    }

    private void Connect() {
        twitchClient = new TcpClient("irc.twitch.tv", 6667);
        reader = new StreamReader(twitchClient.GetStream());
        writer = new StreamWriter(twitchClient.GetStream());
        writer.AutoFlush = true;

        writer.WriteLine("PASS {0}\r\nNICK {1}\r\nUSER {1} 8 * :{1}", password, username);
        //writer.WriteLine("CAP REQ :twitch.tv/membership");
        writer.WriteLine("JOIN #" + channelName);
        lastMessageSendtime = DateTime.Now;
    }

    void TryReceivingMessages() {
        if (twitchClient.Available > 0) {
            var message = reader.ReadLine();

            var iCollon = message.IndexOf(":", 1);
            if (iCollon > 0) {
                var command = message.Substring(1, iCollon);
                if (command.Contains("PRIVMSG #")) {
                    var iBang = command.IndexOf("!");
                    if (iBang > 0) {
                        var speaker = command.Substring(0, iBang);
                        var chatMessage = message.Substring(iCollon + 1);

                        ReceiveMessage(speaker, chatMessage);
                    }
                }
            }
        }
    }

    void ReceiveMessage(string speaker, string message) {
        print(String.Format("\r\n{0}: {1}", speaker, message));

        if (message.ToLower().Contains("!spawn "))
        {
            if (message.ToLower().Contains("enemy"))
            {
                float spawnOffset = 10;
                float posX = enemySpawner.transform.position.x;
                float spawnX = UnityEngine.Random.Range(posX - spawnOffset, posX + spawnOffset);                
                Vector3 spawnPosition = new Vector3(spawnX, enemySpawner.transform.position.y + 0.5f, enemySpawner.transform.position.z);
                GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
                GameManager.instance.AddEnemy(enemy);
            }
        }
    }

    void TrySendingMessages() {
        if (DateTime.Now - lastMessageSendtime > TimeSpan.FromSeconds(2)) {
            if (sendMessageQueue.Count > 0) {
                var message = sendMessageQueue.Dequeue();
                writer.WriteLine(String.Format("{0}{1}", chatMessagePrefix, message));
                lastMessageSendtime = DateTime.Now;
            }
        }
    }
}
