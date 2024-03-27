# skinet
The project used from the course TryCatchLearn

## Table Of Contents

- [Technologies](#technologies)
- [Configurations](#configurations)  
  - [Chocolatey](#chocolatey)
  - [NodeJs](#nodeJs)
  - [HTTPS](#https)
  - [Stripe](#stripe) 

## Technologies
Technologies used in this course:

* .Net 7
  * Entity Framework SQLite 7.0.0
  * AutoMapper 12.0.1
* Angular 16.2.0
* NodeJs 16.14.0

**Optional**
> Only to make it easy to install and configure

* NVM
* Chocolatey
* MKCERT

## Configurations
Configurations that I use to configure and run these applications

### Chocolatey
The main reason to install Chocolatey is because NVM is not supported on Windows.  Chocolatey is a package manager for Windows.

So, nothing better than following the official documentation.
- [Installing Chocolatey](https://chocolatey.org/install)

### NodeJs
Let's use Chocolatey to install and manage the Node versions. Open Powershell as administrator and running this:

``` powershell
# chose desired node version
$version = "8.12.0"
# install nvm w/ cmder
choco install cmder
choco install nvm
refreshenv
# install node
nvm install $version
nvm use $version
```

> [!NOTE]
> You may have a problem executing `refreshenv`, on the console, they'll give you the command necessary to run.

### HTTPS

It's necessary a certificate to run the Angular application, we'll use MKCERT to create a `localhost` certificate.

First, install MKCERT
``` powershell
choco install mkcert
```

Navigate to folder "/client/ssl" and create the certificate.

``` powershell
mkcert -install
```

Then add `localhost` on the certificate

``` powershell
mkcert localhost
```

> [!NOTE]
> It's necessary to execute this process on different machines that need to be configured.

### Stripe

The payment platform that was used in this course and the following configurations are for webhook listening locally.

> [!NOTE]
> These instructions are based on the official documentation [here](https://docs.stripe.com/stripe-cli) for Windows.

1. [download](https://github.com/stripe/stripe-cli/releases/latest) the latest Stripe CLI.
2. Extract `stripe.exe` file, save it in a specific directory, and add it to the environment Path
``` powershell
$Env:Path += ';C:\Tools\stripe'
```
3. Execute the command and you'll be redirected to the website for authentication.
``` powershell
stripe login
```
4. The next command is for starting the local webhook to listen to events. Different events can be listening separated by a comma.
``` powershell
stripe listen -f https://localhost:5001/api/payments/webhook -e payment_intent.succeeded,payment_intent.payment_failed
```
5. When the webhook starts, a key will be provided (the key initiates with "whsec_") and then copy/paste into the property "StripeSettings:WhSecret" on the appsettings.json file.
