<script>
	import { onMount } from 'svelte';
	import store from './store.js';
	import Message from './message.svelte';
	let message;
	let messages = [];

	onMount(() => {
		store.subscribe(currentMessage => {
				messages = [...messages, currentMessage];
		})
	})
	
	function onSendMessage() {
		 if (message.length > 0) {
			 store.sendMessage(message);
			 message = "";
		 }
	}
</script>

<h1>Hello Chat</h1>

<input type="text" bind:value={message} />
<button on:click={onSendMessage}>
	Send Message
</button>
{#each messages as message, i}
		<Message {message} direction={i % 2 == 0 ? "left" :  "right" } />
{/each}

<h2>
	Server Instructions
</h2>
<ol>
	<li>git clone https://github.com/phptuts/node-relay-server.git</li>
	<li>cd node-relay-server</li>
	<li>npm install</li>
	<li>node app.js</li>
	<li>Setup an account with <a href="https://ngrok.com/">ngrok</a>.</li>
	<li>npm install -g ngrok</li>
	<li>ngrok authtoken "your auth token"</li>
	<li>ngrok http 3000</li>
	<li>Go into message-store.js file and replace it with your ngrok url</li>
</ol>

<h3>
	Notes
</h3>

In this svelte tip I use <a href="https://chrome.google.com/webstore/detail/smart-websocket-client/omalebghpgejjiaoknljcfmglgbpocdp">Smart Web Client</a>.

<h2>
	<a href="https://developer.mozilla.org/en-US/docs/Web/API/WebSocket">Docs</a>
</h2>