# What is this?

A project seed for a C# dotnet API ("PaylocityBenefitsCalculator").  It is meant to get you started on the Paylocity BackEnd Coding Challenge by taking some initial setup decisions away.

The goal is to respect your time, avoid live coding, and get a sense for how you work.

# Coding Challenge

**Show us how you work.**

Each of our Paylocity product teams operates like a small startup, empowered to deliver business value in
whatever way they see fit. Because our teams are close knit and fast moving it is imperative that you are able
to work collaboratively with your fellow developers. 

This coding challenge is designed to allow you to demonstrate your abilities and discuss your approach to
design and implementation with your potential colleagues. You are free to use whatever technologies you
prefer but please be prepared to discuss the choices you’ve made. We encourage you to focus on creating a
logical and functional solution rather than one that is completely polished and ready for production.

The challenge can be used as a canvas to capture your strengths in addition to reflecting your overall coding
standards and approach. There’s no right or wrong answer.  It’s more about how you think through the
problem. We’re looking to see your skills in all three tiers so the solution can be used as a conversation piece
to show our teams your abilities across the board.

Requirements will be given separately.

## Requirements

Able to view employees and their dependents
• An employee may only have 1 spouse or domestic partner (not both) 
• An employee may have an unlimited number of children (1 to many)


• Able to calculate and view a paycheck for an employee given the following rules:
o    26 paychecks per year with deductions spread as evenly as possible on each paycheck
o   employees have a base cost of $1,000 per month (for benefits)
o   each dependent represents an additional $600 cost per month (for benefits)
o   employees that make more than $80,000 per year will incur an additional 2% of their yearly salary in
benefits costs
o dependents that are over 50 years old will incur an additional $200 per month

## Requirements (My summary)

- Goal: Calculate/view paycheck (POST/GET)
- Business Rules
    - 1 spouse/domestic partner (NOT both)
    - Unlimited children
    - 26 paychecks/yr (deductions spread evenly)
    - Base: $1000/month
    - Dependent = $600/month (each dependent)
    - 80,000+ salary 
        - 2% of yearly salary in benefits
    - Over 50 > $200 per month

## My Notes / Thoughts

- First approach: How to relate dependent to employee without having a JOIN table that would create the relationship between the 2?
    - Solution I came up with: 
        - Seperate data.json files for Dependents and Employees
        - Added Dictionary for id mapping of employees to dependents
        - Was not sure if could remove Dependents list from GetEmployeeDTO
- Second Approach: in-memory database
    - Thought I could create a JOIN table but, from the research i did, this is a limitation of an in-memory db 
    - Would still have had the GetEmployeeDTO issue (e.g. whether i can edit it)
    - Getting a dependent by id is quite inefficient as all employees need to be fetched to loop through and find the correct dependent
        - This is why I wanted to separate the dependents from employees
- Would DEFINITELY add tests for endpoints and the Benefits class
    - Benefits class (aka Benefits Calculator class) would be prioritized

