using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using UnityEngine;
using Random = UnityEngine.Random;

public class TwitchChat : MonoBehaviour
{
	private TcpClient _twitchClient;
	private StreamReader _reader;
	private StreamWriter _writer;

	private string _username, _channelName, _chatMessagePrefix; //Get the password from https://twitchapps.com/tmi
	private string _channel, _password;
	private DateTime _lastMessageSendtime;

	public GameObject EnemySpawner;
	public GameObject EnemyPrefab;

	private Queue<string> _sendMessageQueue;

	private GameManager _gameManager;

	private void Awake()
	{
		_gameManager = GameManager.Instance();
		_channel = _gameManager.GetUsername();
		_password = _gameManager.GetPassword();
	}

	private void Start()
	{
		_sendMessageQueue = new Queue<string>();
		_username = _channel.ToLower();
		_channelName = _username;
		_chatMessagePrefix = string.Format(":{0}!{0}@{0}.tmi.twitch.tv PRIVMSG #{1} :", _username, _channelName);

		Connect();
	}

	private void SendTwitchMessage(string message)
	{
		//SendTwitchMessage(String.Format("Hello, {0}", speaker));
		_sendMessageQueue.Enqueue(message);
	}

	private void Update()
	{
		if (!_twitchClient.Connected)
		{
			Connect();
		}

		TryReceivingMessages();
		TrySendingMessages();
	}

	private void Connect()
	{
		_twitchClient = new TcpClient("irc.twitch.tv", 6667);
		_reader = new StreamReader(_twitchClient.GetStream());
		_writer = new StreamWriter(_twitchClient.GetStream());
		_writer.AutoFlush = true;

		_writer.WriteLine("PASS {0}\r\nNICK {1}\r\nUSER {1} 8 * :{1}", _password, _username);
		//writer.WriteLine("CAP REQ :twitch.tv/membership");
		_writer.WriteLine("JOIN #" + _channelName);
		_lastMessageSendtime = DateTime.Now;
	}

	private void TryReceivingMessages()
	{
		if (_twitchClient.Available > 0)
		{
			var message = _reader.ReadLine();
			if (string.IsNullOrEmpty(message))
				return;
			var iCollon = message.IndexOf(':', 1);
			if (iCollon > 0)
			{
				var command = message.Substring(1, iCollon);
				if (command.Contains("PRIVMSG #"))
				{
					var iBang = command.IndexOf('!');
					if (iBang > 0)
					{
						var speaker = command.Substring(0, iBang);
						var chatMessage = message.Substring(iCollon + 1);

						ReceiveMessage(speaker, chatMessage);
					}
				}
			}
		}
	}

	private void ReceiveMessage(string speaker, string message)
	{
		print(string.Format("\r\n{0}: {1}", speaker, message));

		if (message.ToLower().Contains("!spawn "))
		{
			if (message.ToLower().Contains("enemy"))
			{
				float spawnOffset = 10;
				var posX = EnemySpawner.transform.position.x;
				var spawnX = Random.Range(posX - spawnOffset, posX + spawnOffset);
				var spawnPosition = new Vector3(spawnX, EnemySpawner.transform.position.y + 0.5f, EnemySpawner.transform.position.z);
				var enemy = Instantiate(EnemyPrefab, spawnPosition, Quaternion.identity);
				_gameManager.AddEnemy(enemy);
			}
		}
	}

	private void TrySendingMessages()
	{
		if (DateTime.Now - _lastMessageSendtime > TimeSpan.FromSeconds(2))
		{
			if (_sendMessageQueue.Count > 0)
			{
				var message = _sendMessageQueue.Dequeue();
				_writer.WriteLine("{0}{1}", _chatMessagePrefix, message);
				_lastMessageSendtime = DateTime.Now;
			}
		}
	}
}