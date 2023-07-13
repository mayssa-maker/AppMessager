<script>

  let email = '';
  let password = '';

  async function handleLogin() {
    try {
      const response = await fetch('https://localhost:7100/login', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({
          email,
          password
        })
      });

      if (response.ok) {
        const data = await response.json();
        const token = data.token;
        // Store the token or perform any other actions
      } else {
        // Handle login error
        loginError = 'Invalid email or password. Please try again.';
      }
    } catch (error) {
      // Handle network or server error
      loginError = 'Login failed. Please try again.';
    }
  }
  
</script>

<h1>Login</h1>

<form on:submit|preventDefault={handleLogin}>
  <label for="email">Email:</label>
  <input type="email" id="email" bind:value={email} />

  <label for="password">Password:</label>
  <input type="password" id="password" bind:value={password} />

  <button type="submit">Login</button>
</form>
