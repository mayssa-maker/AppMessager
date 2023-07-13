<script>
  import { goto } from "$app/navigation";
  let email = "";
  let password = "";
  let loginError = "";

  async function handleLogin() {
    try {
      const response = await fetch("https://localhost:7100/Auth/login", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({
          email,
          password,
        }),
      });

      if (response.ok) {
        const data = await response.json();
        const token = data.token;
        localStorage.setItem("token", token);
        // Login successful
      } else {
        // Handle login error
        loginError = "Invalid email or password. Please try again.";
      }
    } catch (error) {
      // Handle network or server error
      loginError = "Login failed. Please try again.";
    }
     await goto('/');
  }
</script>



<form on:submit|preventDefault={handleLogin}>
    <h1 class="h3 mb-3 fw-normal">Please sign in</h1>

    <input bind:value={email} type="email" class="form-control" placeholder="Email" required>

    <input bind:value={password} type="password" class="form-control" placeholder="Password" required>

    <button class="w-100 btn btn-lg btn-primary" type="submit">Sign in</button>
</form>



