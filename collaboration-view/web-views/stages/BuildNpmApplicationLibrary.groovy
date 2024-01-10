/* Copyright 2019 EPAM Systems.

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at
http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.

See the License for the specific language governing permissions and
limitations under the License.*/

package com.epam.edp.stages.impl.ci.impl.build


import com.epam.edp.stages.impl.ci.ProjectType
import com.epam.edp.stages.impl.ci.Stage

@Stage(name = "build", buildTool = "npm", type = [ProjectType.APPLICATION, ProjectType.LIBRARY])
class BuildNpmApplicationLibrary {
    Script script

    void run(context) {
        script.dir("${context.workDir}") {
            script.withCredentials([script.usernamePassword(credentialsId: "${context.nexus.credentialsId}", passwordVariable: 'PASSWORD',
                    usernameVariable: 'USERNAME')]) {
                def token = script.sh(script: """
        curl -s -H "Accept: application/json" -H "Content-Type:application/json" -X PUT --data \
        '{"name": "${script.USERNAME}", "password": "${script.PASSWORD}"}' \
        ${context.buildTool.groupRepository}-/user/org.couchdb.user:${script.USERNAME} | \
        grep -oE 'NpmToken\\.[0-9a-zA-Z-]+'
        """,
                        returnStdout: true)
            }
            script.sh(script: """
            set +x
            npm set registry ${context.buildTool.groupRepository}
            """)

            script.sh "npm install && npm run build"
        }
    }
}
return BuildNpmApplicationLibrary