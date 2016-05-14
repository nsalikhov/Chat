$(function () {
	var ChatViewModel = function (webSocket) {
		var self = this;

		self.messages = ko.observableArray();
		self.users = ko.observableArray();
		self.message = ko.observable();

		self.onMessage = function (event) {
			if (event.Type === "UsersList") {
				self.users.removeAll();

				ko.utils.arrayPushAll(self.users, event.Data.Users);

				self.displayMessage(event.Data.UpdateMessage, "Notice");
			} else if (event.Type === "Message") {
				var now = new Date();

				var recipient = event.Data.Recipient
					? String.format(" to <{0}> ", event.Data.Recipient)
					: " ";
				var msg = String.format(
					"[{0}:{1}:{2}] <{3}>{4}{5}",
					("0" + now.getHours()).slice(-2),
					("0" + now.getMinutes()).slice(-2),
					("0" + now.getSeconds()).slice(-2),
					event.Data.Sender,
					recipient,
					event.Data.Text);

				self.displayMessage(msg, event.Data.Type);
			}
		};

		self.displayMessage = function (message, type) {
			self.messages.push({ text: message, type: type });
		}

		self.sendMessage = function () {
			var message = self.message();
			if (message.length === 0) {
				return;
			}

			var privateMsgPattern = /^\/msg ([^ .]*) (.*)$/i;

			var privateMessage = privateMsgPattern.exec(message);
			if (privateMessage != null) {
				webSocket.send(JSON.stringify({ type: "Private", Recipient: privateMessage[1], text: privateMessage[2] }));
			} else {
				webSocket.send(JSON.stringify({ type: "Public", text: self.message() }));
			}

			self.message("");
		};

		self.onEnter = function (elem, e) {
			if (e.keyCode === 13) {
				self.sendMessage();
			}
		}
	};

	var socket = new WebSocket("ws://localhost/SimpleChat/Chat/Sync");
	var viewModel = new ChatViewModel(socket);

	ko.applyBindings(viewModel, document.getElementById("chat-container"));

	socket.onmessage = function (event) {
		viewModel.onMessage(JSON.parse(event.data));

		$(".chat-messages").scrollTop($("#messages-table").height());
	};

	socket.onopen = function () {
		viewModel.displayMessage("Welcome to chat!", "Notice");
		viewModel.displayMessage("You can send a private message to another user with the command: /msg nick message.", "Notice");
	};

	socket.onerror = function (e) {
		viewModel.displayMessage(String.format("Socket error occurred: {0}.", e.data), "Error");
	};

	socket.onclose = function () {
		viewModel.displayMessage("Socket connection closed.", "Notice");
	};
});