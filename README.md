Competition Programming Newbies Bot
===================================

This bot is designed to help newcomers in the competitive programming world to learn the new terms, concepts and techniques used,
and to direct them to useful webpages and websites so they can learn more.

How to contribute
-----------------

Everyone is welcome to contribute. Fork it and create a pull request!

The main contribution site is the `definitions.txt` file. You can create new definitions using a simple syntax created by it.
To add a definition, edit the `definitions.txt` and add the following lines:

	## to add a comment, use a double hash
    definition
	pattern: <a regex describing the definition you want to describe; the engine used is the .NET Core one>
	title: <the title of the definition>
	description: <a succint or elaborate description of the definition>
	link: <a link to an article or website describing it more>
	link: <you can add as many links as you want>

Other contributions to the code are also welcome!