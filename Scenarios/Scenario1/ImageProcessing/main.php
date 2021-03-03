<h1>Hello from php!</h1>
<h2>Form example 1</h2>
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

<h2>Form example 2</h2>
<form id="form2" action="form2" method="post">
	<input type="text" name="text1"/>
	<input type="text" name="text2"/>
    <input type="radio" name="radio"/>
	<input type="submit" value="Submit"/>
</form>

<h2><a href="form3">Form example 3</a></h2>

<h2>Form example 4</h2>
	<form id="form4" action="form4" method="get">
	<input type="file" name="file1"/>
    <input type="file" name="file2"/>
	<input type="submit" value="Submit"/>
</form>