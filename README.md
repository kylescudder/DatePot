# DatePot

[![.NET](https://github.com/kylescudder/DatePot/actions/workflows/dotnet.yml/badge.svg)](https://github.com/kylescudder/DatePot/actions/workflows/dotnet.yml)
[![🚀 Deploy on push](https://github.com/kylescudder/DatePot/actions/workflows/main.yml/badge.svg)](https://github.com/kylescudder/DatePot/actions/workflows/main.yml)

## What is The Date Pot
The Date Pot allows Kyle and Rhiann to add films to their To Watch list, restuarants to their eat list, activities to their To Do list, Coffee to the to drink list and Vinyls to their Spend too much money on hisper stuff list.
The film pot allows random file selections so there are no arguments.

## Why make Date Pot
The Date Pot started as just the film pot and was made as a digital replication of their Film Pot so they could continue to watch films from it during Lockdown 2: Electric Boogaloo. Turns out even personal projects are immune from scope creep.
Now it serves as a one stop shop for everything that requires us to keep track over something over the course of months or even years.

### This is the home page
![HomePage](https://i.imgur.com/VJnCxf9.png)

### Adding a film
![AddFilm](https://i.imgur.com/8b6fL0P.png)

### Editing an existing film
![EditFilm](https://i.imgur.com/znTsnvs.png)

The process for the other pots follows the same formula. The coffee pot differs slightly though with a star rating system:

![CoffeePot](https://i.imgur.com/kDnLqD9.png)

## Tech/framework used
Build using .NET Core and for use with a Microsoft SQL Server database using Dapper for the ORM.

## Features
- Each pot allows users to add that thing to a list containing basic information (name, release date, director)
- Random select a film to watch, which if accepted, flags it as watched and removes it from the potential list of films to pick from in the future.
- Allows users to archive items added in error or just wants to get rid of.

## Contribute
If you want to use this for you and yours nothing would make me happier.

MIT © kylescudder

