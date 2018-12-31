# PartY
PartY Inventory Management System User Guide

## To begin using PartY:

1. Ensure you have a license for MS SQLServer (may have built-in SQLite in the future)
2. open PartY.exe
3. Select menu dropdown File > Select...
4. Open PartYInventory.mdf

## SALES TAB:
	left column:
		image of item
			-set by user in add/modify tab
		cart
			-when item added to cart, it appears here
	center column:
		size of item
		NSFW flag
		venue selection
		discount $
		discount checkbox
		tax %
		tax flag
		finalize sale of items in cart button
		cancel sale button
		total price display
	right column:
		list of inventory
			-select items then click add to cart

## STATISTICS TAB:
	left column:
		<data> by <frame> analysis
			-allows you to see how well sales are per time frame or venue
	right column:
		top charts
			-shows top items
			
## RESTOCK TAB:
	left column:
		list of inventory
		quantity to add
		update button
		set whether item is discontinued
			-currently does nothing, but future update planned for notifying when items are low in stock
			-by default is resume restock, but if discontinued, in the future, there will be no notification if stock is low

## ADD/MODIFY TAB:
	left column:
		Add New Price
			-add a name of a size and its price, for example 11x17 print, or medium shirt
		size name
		size price
		cost to manufacture
			-used to determine profits by subtracting this from its price
		add price button
		list of prices
	hard restore functions:
		save database as CSV
			-dump all DB tables and their data into a CSV file
		overwrite database with CSV
			-take all info from CSV files and replace DB with CSV data
	center column:
		Add New Merch
			-add a merchandise item which will be displayed in the sales tab
		item name
		select size
		NSFW checkbox
		enter quantity currently in stock
		add merch button
		merchandise display
		remove merchandise button
		add image for merchandise
			-by default no image, but image can be added or changed to assist in sales
	right column:
		Add New Venue
			-creates a venue to keep track of where your sales happen. Might associate a sales tax with a venue in future update
		venue name
		add venue button
		venue list
		remove venue button
		
## ABOUT TAB:
	PartY logo
	Version information
	TODO
		-might remove this in the future if updates are more frequent or unpredictable
