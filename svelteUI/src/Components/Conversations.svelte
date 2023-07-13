<script>
  let conversations = [];

  // Fetch conversations from the backend API
  async function fetchConversations() {
    try {
      const response = await fetch(' https://localhost:7100/Auth/conversations');
      if (response.ok) {
        conversations = await response.json();
      } else {
        console.error('Failed to fetch conversations');
      }
    } catch (error) {
      console.error('Error fetching conversations:', error);
    }
  }

  // Call the fetchConversations function when the component is mounted
  onMount(fetchConversations);
</script>

<h1>Conversations</h1>

<ul>
  {#each conversations as conversation}
    <li>{conversation.title}</li>
  {/each}
</ul>
