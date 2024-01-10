FROM ubuntu:18.04

# To make it easier for build and release pipelines to run apt-get,
# configure apt to not require confirmation (assume the -y argument by default)
ENV DEBIAN_FRONTEND=noninteractive
RUN echo "APT::Get::Assume-Yes \"true\";" > /etc/apt/apt.conf.d/90assumeyes

RUN apt-get update && apt-get install -y --no-install-recommends \
    ca-certificates \
    curl \
    jq \
    git \
    iputils-ping \
    libcurl4 \
    libicu60 \
    libunwind8 \
    netcat \
    libssl1.0 \
    zip \
    unzip \
    wget \
    apt-transport-https \
    gnupg \
    gnupg-agent \
    software-properties-common \
    apt-utils \
    ssh \
    openjdk-11-jre \
    gettext-base \
    tzdata \
    && rm -rf /var/lib/apt/lists/*

RUN curl -LsS https://aka.ms/InstallAzureCLIDeb | bash \
    && rm -rf /var/lib/apt/lists/*

# Download the Microsoft repository GPG keys
RUN wget -q https://packages.microsoft.com/config/ubuntu/18.04/packages-microsoft-prod.deb
RUN dpkg -i packages-microsoft-prod.deb
# Update the list of packages after we added packages.microsoft.com
RUN apt-get update && apt-get install -y powershell \
    && /usr/bin/pwsh -NoLogo -NoProfile -NonInteractive -Command Install-Module Az -Force -Scope AllUsers

# Install Terraform 1.0.9
RUN wget -q https://releases.hashicorp.com/terraform/1.1.6/terraform_1.1.6_linux_amd64.zip \
&& unzip terraform_1.1.6_linux_amd64.zip \
&& chmod +x ./terraform \
&& mv ./terraform /usr/local/bin/terraform

# Install all build tools in /build-tools
WORKDIR /build-tools

# Install Azure CLI - ID09262019
RUN curl -sL https://aka.ms/InstallAzureCLIDeb | bash

# Install kubectl (latest)
RUN curl -LO https://storage.googleapis.com/kubernetes-release/release/`curl -s https://storage.googleapis.com/kubernetes-release/release/stable.txt`/bin/linux/amd64/kubectl \
&& chmod +x ./kubectl \
&& mv ./kubectl /usr/local/bin/kubectl

# Install helm v3 (latest)
RUN curl -fsSL -o get_helm.sh https://raw.githubusercontent.com/helm/helm/master/scripts/get-helm-3 \
&& chmod 700 get_helm.sh \
&& ./get_helm.sh

ARG TARGETARCH=amd64
ARG AGENT_VERSION=2.198.2

WORKDIR /azp
RUN if [ "$TARGETARCH" = "amd64" ]; then \
      AZP_AGENTPACKAGE_URL=https://vstsagentpackage.azureedge.net/agent/${AGENT_VERSION}/vsts-agent-linux-x64-${AGENT_VERSION}.tar.gz; \
    else \
      AZP_AGENTPACKAGE_URL=https://vstsagentpackage.azureedge.net/agent/${AGENT_VERSION}/vsts-agent-linux-${TARGETARCH}-${AGENT_VERSION}.tar.gz; \
    fi; \
    curl -LsS "$AZP_AGENTPACKAGE_URL" | tar -xz

COPY ./start.sh .
RUN chmod +x start.sh

ENTRYPOINT [ "./start.sh" ]