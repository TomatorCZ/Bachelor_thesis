<h1>Hello from php!</h1>
<p>Form example 1</p>
<form method="get" action="form1">
    <label>Username:</label>
    <input name="username" type="text" required="required"/><br/>
    <label>Email:</label>
    <input name="email" type="email"><br/>
    <label>Age:</label>
    <input name="age" type="number" min="0"/><br/>
    <label>Comment:</label>
    <textarea name="comment" required="required"></textarea><br/>
    <label>Publish:</label>
    <select name="publish">
        <option value="private" selected>private</option>
        <option value="public">public</option>
    </select><br/>
    <input type="submit" value="Submit"/><br/>
</form>
<p>Form example 2</p>
<form id="form2" action="form2" method="post">
	<input type="text" name="text1"/>
	<input type="text" name="text2"/>
	<input type="submit" value="Submit"/>
</form>
<p>Form example 3</p>
	<form id="form3" action="form3" method="get">
	<input type="file" name="file1"/>
	<input type="submit" value="Submit"/>
</form>