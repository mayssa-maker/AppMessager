<script>
  // Import necessary dependencies or functions here

  let username = '';
  let email = '';
  let password = '';

  
  async function handleRegistration() {
   try {
      const response = await fetch('https://localhost:7100/register', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({
          username,
          email,
          password
        })
      });

      if (response.ok) {
        const data = await response.json();
        const token = data.token;
        // Store the token or perform any other actions
      } else {
        // Handle registration error
        registrationError = 'Registration failed. Please try again.';
      }
    } catch (error) {
      // Handle network or server error
      registrationError = 'Registration failed. Please try again.';
    }
  }
</script>

<h1>Registration</h1>

<form on:submit|preventDefault={handleRegistration}>
  <label for="username">Username:</label>
  <input type="text" id="username" bind:value={username} />

  <label for="email">Email:</label>
  <input type="email" id="email" bind:value={email} />

  <label for="password">Password:</label>
  <input type="password" id="password" bind:value={password} />

  <button type="submit">Register</button>
</form>
