using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class TwitchChat : Singleton<TwitchChat>
{
	private bool _isInitializing = false;
	private bool _isInitialized = false;

	private TcpClient _twitchClient;
	private StreamReader _reader;
	private StreamWriter _writer;

	private string _username, _channel, _password, _chatMessagePrefix;
	private DateTime _lastMessageSendtime;

	private GameObject _enemySpawner;
	public GameObject EnemyPrefab;

	private Queue<string> _sendMessageQueue;

	private GameManager _gameManager;

	private void Awake()
	{
		_gameManager = GameManager.Instance();
	}

	private void Start()
	{
		_sendMessageQueue = new Queue<string>();
	}

	private IEnumerator Initialize()
	{
		_isInitializing = true;
		_channel = _gameManager.Channel;
		_username = "pixelsrealmtowerdefense";
		_password = "oauth:7e7ld6w6sgcazxq9srtwlsh283vu4h";
		_chatMessagePrefix = string.Format(":{0}!{0}@{0}.tmi.twitch.tv PRIVMSG #{1} :", _username, _channel);
		while (_twitchClient == null || !_twitchClient.Connected)
		{
			Connect();
			yield return 0;
		}
		SendTwitchMessage("Game Started");
		_isInitializing = false;
		_isInitialized = true;
	}

	public void SendTwitchMessage(string message)
	{
		_sendMessageQueue.Enqueue(message);
	}

	private void Update()
	{
		if (!_isInitialized)
		{
			if (!_isInitializing && SceneManager.GetActiveScene().buildIndex > 0)
				StartCoroutine(Initialize());
			return;
		}
		if (_isInitializing)
		{
			return;
		}

		if (!_enemySpawner)
			_enemySpawner = GameObject.FindWithTag("Enemy Spawner");

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
		_writer.WriteLine("JOIN #" + _channel);
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
		Debug.LogFormat("Received message: {0}: {1}", speaker, message);

		if (message.ToLower().Contains("!spawn "))
		{
			if (message.ToLower().Contains("enemy"))
			{
				float spawnOffset = 10;
				var posX = _enemySpawner.transform.position.x;
				var spawnX = Random.Range(posX - spawnOffset, posX + spawnOffset);
				var spawnPosition = new Vector3(spawnX, _enemySpawner.transform.position.y + 0.5f, _enemySpawner.transform.position.z);
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
				Debug.Log("Sent Message: " + message);
			}
		}
	}
}